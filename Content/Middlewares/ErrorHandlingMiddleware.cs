using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.Exceptions;

namespace WebApiTemplate.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate next = next;
    private const string ContentTypeProblemJson = "application/problem+json";

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int status = exception switch
        {
            NotFoundException _ => StatusCodes.Status404NotFound,
            BadRequestException _ => StatusCodes.Status400BadRequest,
            NotImplementedException _ => StatusCodes.Status501NotImplemented,
            _ => StatusCodes.Status500InternalServerError,
        };

        var problems = new ProblemDetails
        {
            Title = status.ToString(),
            Status = status,
            Detail = exception.Message
        };

        string result = JsonSerializer.Serialize(problems);
        context.Response.ContentType = ContentTypeProblemJson;
        context.Response.StatusCode = status;
        await context.Response.WriteAsync(result);
        return;
    }
}
