using Application.Models.User;
using Infras.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Report.Controllers
{
    [ApiController]
    [Route("radar-report")]
    public class RadarReport : ControllerBase
    {
        private readonly ICleanUpBlobService _cleanUpBlobService;
        private readonly ITableStorageService _tableStorageService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<RadarReport> _logger;

        public RadarReport(
            ITableStorageService tableStorageService,
            ICleanUpBlobService cleanUpBlobService,
            ILogger<RadarReport> logger
            )
        {
            _tableStorageService = tableStorageService;
            _cleanUpBlobService = cleanUpBlobService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            await _cleanUpBlobService.CleanUpBlobAsync();
           
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserEntity user)
        {
            await _tableStorageService.AddUserAsync(user);

            return CreatedAtAction(nameof(GetUser), new { partitionKey = user.PartitionKey, rowKey = user.RowKey }, user);
        }

        [HttpGet("user/{partitionKey}/{rowKey}")]
        public async Task<IActionResult> GetUser(string partitionKey, string rowKey)
        {
            var user = await _tableStorageService.GetUserAsync(partitionKey, rowKey);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpDelete("{partitionKey}/{rowKey}")]
        public async Task<IActionResult> DeleteUser(string partitionKey, string rowKey)
        {
            await _tableStorageService.DeleteUserAsync(partitionKey, rowKey);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserEntity user)
        {
            await _tableStorageService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpGet("partition/{partitionKey}")]
        public async Task<IActionResult> GetUserByPartition(string partitionKey)
        {
            var users =  await _tableStorageService.GetEntities(partitionKey);
            if (users == null || users.Count == 0) return NotFound();
            return Ok(users);
        }

        [HttpDelete("partition/{partitionKey}/batch")]
        public async Task<IActionResult> BulkDeleteUsersByPartitionKeyBatch(string partitionKey)
        {
            bool isDeleted = await _tableStorageService.BulkDeleteByPartitionKeyBatchAsync(partitionKey);

            if (!isDeleted) return NotFound($"No users found with partition key '{partitionKey}'.");

            return NoContent();
        }

        [HttpPost("bulk-insert-users")]
        public async Task<IActionResult> BulkInsertUsers([FromBody] List<UserEntity> users)
        {
            bool isInserted = await _tableStorageService.BulkInsertionAsync(users);

            if (!isInserted) return BadRequest("No users were provided for insertion.");

            return Ok(users);
        }
    }
}
