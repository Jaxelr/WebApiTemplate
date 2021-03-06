﻿using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApiTemplate.Exceptions;
using WebApiTemplate.Models;

namespace WebApiTemplate.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(BadRequestException))
            {
                status = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                status = HttpStatusCode.NotFound;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
            }

            var failedResponse = new FailedResponse(exception);

            string result = JsonSerializer.Serialize(failedResponse);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) status;
            return context.Response.WriteAsync(result);
        }
    }
}
