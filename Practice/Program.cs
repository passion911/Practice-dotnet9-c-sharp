using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Practice.Filters;
using Practice.Middleware;
using Services.ServiceRegistration;

namespace Practice;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bao Anh Handsome", Version = "v1" });
        });
        builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        builder.Services.AddApplicationServices();
        builder.Services.TryAddScoped<ApiCustomFilter>();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if ( app.Environment.IsDevelopment() )
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            c.OAuthUsePkce();
            c.OAuthScopeSeparator(" ");
        });

        app.UseAuthorization();

        app.UseMiddleware<RequestLoggingMiddleware>();

        app.MapControllers();

        app.Run();
    }
}
