using Application.Services.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application.Services.BackgroundServices;

public class CleanupObsoletedData : BackgroundService
{
    private readonly ILogger<CleanupObsoletedData> _logger;
    private readonly IServiceProvider _services;

    public CleanupObsoletedData(
        IServiceProvider services,
        ILogger<CleanupObsoletedData> logger
        )
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await ExecuteProcess(cancellationToken);
        }
        catch ( Exception ex )
        {
            _logger.LogError("Operation has ERROR: {0}", ex.Message);
            throw;
        }
        finally
        {
            _logger.LogInformation("Operation shutdown.");
        }
    }

    private async Task ExecuteProcess(CancellationToken cancellationToken)
    {
        while ( !cancellationToken.IsCancellationRequested )
        {
            _logger.LogInformation("Process started...");

            try
            {
                using ( var scope = _services.CreateScope() )
                {
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                    await emailService.SendMailAsync();
                }
            }
            catch ( Exception ex )
            {
                _logger.LogError(ex, "Process has ERROR: {0}", ex.Message);
                throw;
            }

            await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
        }
    }
}
