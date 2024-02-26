using System.Data.SqlClient;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebApiTemplate.Exceptions;
using WebApiTemplate.Models;
using WebApiTemplate.Repositories;

var builder = WebApplication.CreateBuilder(args);

var settings = new AppSettings();

builder.Configuration.GetSection(nameof(AppSettings)).Bind(settings);

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

builder.Services.AddHealthChecks()
        .AddCheck(settings.HealthDefinition.Name,
        () => HealthCheckResult.Healthy(settings.HealthDefinition.HealthyMessage),
        tags: settings.HealthDefinition.Tags);

builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddControllers();

#if !DEBUG
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(settings.AuthenticationName)
                .AddJwtBearer(settings.AuthenticationName, options =>
                {
                    options.Authority = settings.AuthorizationServer;
                    options.RequireHttpsMetadata = false;
                    options.Audience = settings.ServiceName;
                });
#endif

builder.Services.AddLogging(opt =>
{
    opt.AddConsole();
    opt.AddDebug();
    opt.AddConfiguration(builder.Configuration.GetSection("Logging"));
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(settings.Version, new OpenApiInfo
    {
        Title = settings.ServiceName,
        Version = settings.Version
    });
});

//Custom classes
builder.Services.AddSingleton(settings); //typeof(AppSettings)
builder.Services.AddSingleton(new SqlConnection(settings.ConnectionString));
builder.Services.AddSingleton<IRepository>(provider => new Repository(provider.GetService<SqlConnection>()));

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    //Note: Comment as needed.
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseCors(settings.Policy);
app.UseRouting();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks(settings.HealthDefinition.Endpoint, new HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("swagger/v1/swagger.json", $"{settings.ServiceName} API ({settings.Version})");
    c.RoutePrefix = string.Empty;
});

await app.RunAsync();
