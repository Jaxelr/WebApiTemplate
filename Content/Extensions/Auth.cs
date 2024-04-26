using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebApiTemplate.Models;

namespace WebApiTemplate.Extensions;

public static class AuthExtensions
{
    public static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(settings.AuthenticationName)
            .AddJwtBearer(settings.AuthenticationName, options =>
            {
                options.Authority = settings.AuthorizationServer;
                options.RequireHttpsMetadata = false;
                options.Audience = settings.ServiceName;
            });

        return builder;
    }

    public static WebApplication UseAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
