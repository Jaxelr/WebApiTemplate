using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTemplate.Exceptions;

namespace WebApiTemplate.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;
    private const string ContentTypeProblemJson = "application/problem+json";

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

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
        var status = exception switch
        {
            NotFoundException _ => HttpStatusCode.NotFound,
            BadRequestException _ => HttpStatusCode.BadRequest,
            NotImplementedException _ => HttpStatusCode.NotImplemented,
            _ => HttpStatusCode.InternalServerError,
        };

        var problems = new ProblemDetails
        {
            Title = status.ToString(),
            Status = (int) status,
            Detail = exception.Message
        };

        string result = JsonSerializer.Serialize(problems);
        context.Response.ContentType = ContentTypeProblemJson;
        context.Response.StatusCode = (int) status;
        await context.Response.WriteAsync(result);
        return;
    }
}
