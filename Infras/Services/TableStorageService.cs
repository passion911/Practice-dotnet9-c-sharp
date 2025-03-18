using Application.Models.User;
using Azure;
using Azure.Data.Tables;
using Infras.Options;
using Infras.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infras.Services;

public class TableStorageService : ITableStorageService
{
    private readonly ILogger<TableStorageService> _logger;
    private const string _tableName = "users";
    private readonly TableClient _tableClient;
    private readonly int _batchSize = 100;

    public TableStorageService(
        ILogger<TableStorageService> logger,
        IOptions<StorageAccountOption> options
        )
    {
        _logger = logger;
        var settings = options.Value;
        var serviceClient = new TableServiceClient(settings.StorageAccountConnectionString);
        _tableClient = serviceClient.GetTableClient(_tableName);
        _tableClient.CreateIfNotExists();
    }

    public async Task AddUserAsync(UserEntity user)
    {
        await _tableClient.AddEntityAsync(user);
    }

    public async Task<UserEntity?> GetUserAsync(string partitionKey, string rowKey)
    {
        try
        {
            return await _tableClient.GetEntityAsync<UserEntity>(partitionKey, rowKey);
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            return null;
        }
    }

    public async Task DeleteUserAsync(string partitionKey, string rowKey)
    {
        await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    public async Task UpdateUserAsync(UserEntity user)
    {
        await _tableClient.UpdateEntityAsync(user, ETag.All, TableUpdateMode.Replace);
    }

    public async Task<List<UserEntity>> GetEntities(string partitionKey)
    {
        var dataSet =  _tableClient.QueryAsync<UserEntity>(u => u.PartitionKey == partitionKey);
        List<UserEntity> users = [];

        await foreach (var entity in dataSet)
        {
            users.Add(entity);
        }

        return users;
    }

    public async Task<bool> BulkDeleteByPartitionKeyBatchAsync(string partitionKey)
    {
        var entitiesToDelete = await GetEntities(partitionKey);
        if (!entitiesToDelete.Any()) return false;
        int deletedCount = 0;
        var groupedEntities = entitiesToDelete.Select((entity, index) => new { entity, index })
                                         .GroupBy(x => x.index / _batchSize)
                                         .Select(g => g.Select(x => x.entity).ToList())
                                         .ToList();

        foreach (var batch in groupedEntities)
        {
            var transactionActions = new List<TableTransactionAction>();

            foreach (var entity in batch)
            {
                transactionActions.Add(new TableTransactionAction(TableTransactionActionType.Delete, entity));
            }

            await _tableClient.SubmitTransactionAsync(transactionActions);
            deletedCount += batch.Count;
        }
        _logger.LogInformation($"Total deleted items in \"{partitionKey}\" partition: {deletedCount}");
        return true;
    }

    public async Task<bool> BulkInsertionAsync(List<UserEntity> users)
    {
        if (users == null || users.Count == 0) return false;

        var partitionGroups = users.GroupBy(u => u.PartitionKey).ToList();

        foreach (var partitionGroup in partitionGroups)
        {
            var partitionKey = partitionGroup.Key;
            var partitionEntities = partitionGroup.ToList();

            var groupedBatches = partitionEntities.Select((user, index) => new { user, index })
                                                  .GroupBy(x => x.index / _batchSize)
                                                  .Select(g => g.Select(x => x.user).ToList())
                                                  .ToList();

            foreach (var batch in groupedBatches)
            {
                var transactionActions = new List<TableTransactionAction>();

                foreach (var entity in batch)
                {
                    transactionActions.Add(new TableTransactionAction(TableTransactionActionType.Add, entity));
                }

                await _tableClient.SubmitTransactionAsync(transactionActions);
            }
        }

        return true;
    }
}
