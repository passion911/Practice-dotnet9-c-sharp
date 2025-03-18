using Application.Models.User;

namespace Infras.Services.Contracts;

public interface ITableStorageService
{
    public Task AddUserAsync(UserEntity user);
    public Task<UserEntity?> GetUserAsync(string partitionKey, string rowKey);
    public Task DeleteUserAsync(string partitionKey, string rowKey);
    public Task UpdateUserAsync(UserEntity user);
    public Task<List<UserEntity>> GetEntities(string partitionKey);
    public Task<bool> BulkDeleteByPartitionKeyBatchAsync(string partitionKey);
    public Task<bool> BulkInsertionAsync(List<UserEntity> users);
}
