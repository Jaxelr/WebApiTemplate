using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebApiTemplate.Models;

namespace WebApiTemplate.Extensions;

public static class CorsExtension
{
    public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(settings.Policy,
            builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        return builder;
    }

    public static WebApplication UseCors(this WebApplication app, AppSettings settings)
    {
        app.UseCors(settings.Policy);

        return app;
    }
}
