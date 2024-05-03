using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApiTemplate.Exceptions;
using WebApiTemplate.Extensions;
using WebApiTemplate.Models;

var builder = WebApplication.CreateBuilder(args);

var settings = new AppSettings();

builder.Configuration.GetSection(nameof(AppSettings)).Bind(settings);

builder.AddCors(settings);
builder.AddHealthcheck(settings);

builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddControllers();

#if !DEBUG

builder.AddAuth(settings);

#endif

builder.AddLogging();
builder.AddSwagger(settings);

builder.AddDependencies(settings);

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

app.UseCors(settings);
app.UseRouting();

app.UseExceptionHandler();
app.UseAuth();

app.MapControllers();

app.UseHealthcheck(settings);
app.UseSwagger(settings);

await app.RunAsync();
