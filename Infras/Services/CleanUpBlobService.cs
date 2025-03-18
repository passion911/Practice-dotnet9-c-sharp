using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Infras.Options;
using Infras.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infras.Services;

public class CleanUpBlobService : ICleanUpBlobService
{
    private const string _blobContainerName = "report-exporting";
    private readonly StorageAccountOption _storageAccOptions;
    private readonly ILogger<CleanUpBlobService> _logger;

    public CleanUpBlobService(IOptions<StorageAccountOption> storageAccOptions,
        ILogger<CleanUpBlobService> logger)
    {
        _storageAccOptions = storageAccOptions.Value;
        _logger = logger;
    }
    
    public async Task CleanUpBlobAsync()
    {
        string connectionString = _storageAccOptions.StorageAccountConnectionString;
        BlobContainerClient containerClient = new(connectionString, _blobContainerName);

        if (await containerClient.ExistsAsync())
        {
            DateTimeOffset thresholdDate = DateTimeOffset.Now.AddDays(-1);

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                if (blobItem.Properties.LastModified <= thresholdDate)
                {
                    _logger.LogInformation($"Deleting blob: {blobItem.Name}");
                    await containerClient.DeleteBlobIfExistsAsync(blobItem.Name, DeleteSnapshotsOption.IncludeSnapshots);
                }
            }
        }else
        {
            await containerClient.CreateIfNotExistsAsync();
        }

        _logger.LogInformation($"Deleting blobs DONE.");
    }
}
