using Azure;
using Azure.Data.Tables;

namespace Application.Models.User;

public class UserEntity : ITableEntity
{
    public string PartitionKey { get; set; } // Example: "Users"
    public string RowKey { get; set; } // Example: "UserId"
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public ETag ETag { get; set; } = ETag.All;
    public DateTimeOffset? Timestamp { get; set; }
}
