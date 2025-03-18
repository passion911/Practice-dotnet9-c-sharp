using Infras.Options;
using Infras.StorageAccount;
using Microsoft.OpenApi.Models;

namespace Report
{
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Report api", Version = "v1" });
            });

           // builder.Services.Configure<StorageAccountOption>(builder.Configuration.GetSection(StorageAccountOption.SectionName));
            builder.Services.ConfigureStorageAccInfra();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
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


            app.MapControllers();

            app.Run();
        }
    }
}
