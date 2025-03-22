using Application.Services.Email;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services.Contracts;
using Services.Implementations;
using Services.Implementations.Cars;
using Services.MessageConsumer;
using Services.MessagePublisher;

namespace Services.ServiceRegistration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.TryAddScoped<IPaymentService, StripePaymentService>();
        services.TryAddScoped<IOrderProcessorService, OrderProcessorService>();
        services.TryAddScoped<IEmailService, EmailService>();
        services.TryAddScoped<ICheckoutService, CheckoutService>();
        return services;
    }

    public static IServiceCollection AddCarServices(this IServiceCollection services)
    {
        services.TryAddTransient<EconomyHatchback>();
        services.TryAddTransient<EconomySedan>();
        services.TryAddSingleton<ICarFactory>();
        return services;
    }

    public static IServiceCollection AddRabbitMQService(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<ConsumerMessageOfRabbitMQ>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
                cfg.ReceiveEndpoint("my-message-queue", e =>
                {
                    e.ConfigureConsumer<ConsumerMessageOfRabbitMQ>(context);
                });
            });
        });
        return services;
    }

    public static IServiceCollection AddPublisherService(this IServiceCollection services)
    {
        services.AddScoped<EventHubPublisher>();
        return services;
    }
}




