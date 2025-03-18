using Infras.Options;
using Infras.Services.Contracts;
using Infras.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infras.StorageAccount;

public static class BootStraper
{
    public static IServiceCollection ConfigureStorageAccInfra(this IServiceCollection services)
    {
        services.AddOptions<StorageAccountOption>().Configure<IConfiguration>((options, configuration) =>
        {
            configuration.GetSection(nameof(StorageAccountOption)).Bind(options);
            string connString = configuration.GetValue<string>("StorageAccountOption:StorageAccountConnectionString")!;
            options.StorageAccountConnectionString = connString;
        });
        services.AddScoped<ICleanUpBlobService, CleanUpBlobService>();
        services.AddScoped<ITableStorageService, TableStorageService>();

        return services;
    }
}
