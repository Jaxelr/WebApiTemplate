using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTemplate.Exceptions;

public class ExceptionHandler : IExceptionHandler
{
    private const string ContentTypeProblemJson = "application/problem+json";

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
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
        httpContext.Response.ContentType = ContentTypeProblemJson;
        httpContext.Response.StatusCode = status;

        await httpContext.Response.WriteAsync(result, cancellationToken: cancellationToken);

        return true;
    }
}
