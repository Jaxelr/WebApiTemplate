using System.Data.SqlClient;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using WebApiTemplate.Middlewares;
using WebApiTemplate.Models;
using WebApiTemplate.Repositories;

namespace WebApiTemplate
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private readonly AppSettings settings;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(env.ContentRootPath)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();

            Configuration = builder.Build();

            //Extract the AppSettings information from the appsettings config.
            settings = new AppSettings();
            Configuration.GetSection(nameof(AppSettings)).Bind(settings);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
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

            //HealthChecks
            services.AddHealthChecks();

            services.AddControllers();

            services.AddAuthorization();
            services.AddAuthentication(settings.AuthenticationName)
                .AddJwtBearer(settings.AuthenticationName, options =>
                {
                    options.Authority = settings.AuthorizationServer;
                    options.RequireHttpsMetadata = false;
                    options.Audience = settings.ServiceName;
                });

            services.AddLogging(opt =>
            {
                opt.AddConsole();
                opt.AddDebug();
                opt.AddConfiguration(Configuration.GetSection("Logging"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(settings.Version, new OpenApiInfo
                {
                    Title = settings.ServiceName,
                    Version = settings.Version
                });
            });

            //Your custom classes go here.
            services.AddSingleton(settings); //typeof(AppSettings)
            services.AddSingleton(new SqlConnection(settings.ConnectionString));
            services.AddSingleton<IRepository>(provider => new Repository(provider.GetService<SqlConnection>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //Note: Comment as needed.
                app.UseHttpsRedirection();
            }

            app.UseCors(settings.Policy);
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", $"{settings.ServiceName} API ({settings.Version})");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
