using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApiTemplate.Extensions;

public static class LoggingExtensions
{
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.Services.AddLogging(opt =>
        {
            opt.AddConsole();
            opt.AddDebug();
            opt.AddConfiguration(builder.Configuration.GetSection("Logging"));
        });

        return builder;
    }
}
