using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using WebApiTemplate.Models;

namespace WebApiTemplate.Extensions;

public static class SwaggerExtensions
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(settings.Version, new OpenApiInfo
            {
                Title = settings.ServiceName,
                Version = settings.Version
            });
        });

        return builder;
    }

    public static WebApplication UseSwagger(this WebApplication app, AppSettings settings)
    {
        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("swagger/v1/swagger.json", $"{settings.ServiceName} API ({settings.Version})");
            c.RoutePrefix = string.Empty;
        });

        return app;
    }
}
