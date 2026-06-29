using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Template.WebApi.Clean.Infrastructure.Exceptions;

namespace Template.WebApi.Clean.Infrastructure.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path
            };

            LogLevel logLevel;
            int statusCode;

            switch (exception)
            {
                case CustomExceptionProjectException customEx:
                    statusCode = (int)customEx.StatusCode;
                    problemDetails.Title = customEx.Message;
                    logLevel = LogLevel.Error;
                    break;
                case BadRequestProjectException:
                    statusCode = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Bad Request";
                    logLevel = LogLevel.Information;
                    break;
                case ForbiddenProjectException:
                    statusCode = StatusCodes.Status403Forbidden;
                    problemDetails.Title = "Forbidden";
                    logLevel = LogLevel.Information;
                    break;
                case NotFoundProjectException:
                    statusCode = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Not Found";
                    logLevel = LogLevel.Information;
                    break;
                case TimeoutProjectException:
                    statusCode = StatusCodes.Status408RequestTimeout;
                    problemDetails.Title = "Request Timeout";
                    logLevel = LogLevel.Warning;
                    break;
                case UnauthorizedAccessProyectException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    problemDetails.Title = "Unauthorized";
                    logLevel = LogLevel.Information;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Internal Server Error";
                    logLevel = LogLevel.Error;
                    break;
            }

            problemDetails.Status = statusCode;
            problemDetails.Detail = exception.Message;

            if (exception is ProjectException projectEx)
            {
                if (!string.IsNullOrEmpty(projectEx.Detail))
                    problemDetails.Detail = projectEx.Detail;

                if (!string.IsNullOrEmpty(projectEx.Module))
                    problemDetails.Extensions["module"] = projectEx.Module;

                logLevel = projectEx.WithLogError ? LogLevel.Error : LogLevel.Information;
            }

            _logger.Log(logLevel, exception, "{Title}: {Message}", problemDetails.Title, problemDetails.Detail);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var result = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(result);
        }
    }
}
