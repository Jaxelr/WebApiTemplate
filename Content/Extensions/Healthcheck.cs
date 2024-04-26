using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApiTemplate.Models;

namespace WebApiTemplate.Extensions;

public static class HealthcheckExtensions
{
    public static WebApplicationBuilder AddHealthcheck(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddHealthChecks()
                .AddCheck(settings.HealthDefinition.Name,
                () => HealthCheckResult.Healthy(settings.HealthDefinition.HealthyMessage),
                tags: settings.HealthDefinition.Tags);

        return builder;
    }

    public static WebApplication UseHealthcheck(this WebApplication app, AppSettings settings)
    {
        app.UseHealthChecks(settings.HealthDefinition.Endpoint, new HealthCheckOptions()
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}
