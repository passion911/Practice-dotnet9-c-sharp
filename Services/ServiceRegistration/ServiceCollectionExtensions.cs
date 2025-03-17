using Application.Services.Email;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services.Contracts;
using Services.Implementations;

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
}




