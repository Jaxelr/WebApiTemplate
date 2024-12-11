using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using WebApiTemplate.Models;
using WebApiTemplate.Repositories;

namespace WebApiTemplate.Extensions;

public static class DependencyExtensions
{
    public static WebApplicationBuilder AddDependencies(this WebApplicationBuilder builder, AppSettings settings)
    {
        //Custom classes
        builder.Services.AddSingleton(settings); //typeof(AppSettings)
        builder.Services.AddSingleton(new SqlConnection(settings.ConnectionString));
        builder.Services.AddSingleton<IRepository>(provider => new Repository(provider.GetService<SqlConnection>()));

        return builder;
    }
}
