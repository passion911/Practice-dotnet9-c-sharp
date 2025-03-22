using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Services.MessageConsumer;

public class ConsumerMessageOfRabbitMQ : BackgroundService, IConsumer<string>
{
    private readonly ILogger<ConsumerMessageOfRabbitMQ> _logger;

    public ConsumerMessageOfRabbitMQ(ILogger<ConsumerMessageOfRabbitMQ> logger)
    {
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<string> context)
    {
        _logger.LogInformation("Received message: {Message}", context.Message);
        return Task.CompletedTask;
    }
}
