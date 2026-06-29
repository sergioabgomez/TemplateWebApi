using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Template.WebApi.Clean.Infrastructure.Exceptions;
using Template.WebApi.Clean.Infrastructure.Models;
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
            var code = HttpStatusCode.InternalServerError;
            var message = "An error occurred while processing your request.";
            _logger.LogError(exception, "An unhandled exception occurred.");
            switch (exception)
            {
                case CustomExceptionProjectException customEx:
                    code = customEx.StatusCode;
                    message = customEx.Message;
                    _logger.LogError(customEx, "Custom exception: {Message}", customEx.Message);
                    break;
                case BadRequestProjectException:
                    code = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
                case ForbiddenProjectException:
                    code = HttpStatusCode.Forbidden;
                    message = exception.Message;
                    break;
                case NotFoundProjectException:
                    code = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
                case TimeoutProjectException:
                    code = HttpStatusCode.RequestTimeout;
                    message = exception.Message;
                    break;
                case UnauthorizedAccessProyectException:
                    code = HttpStatusCode.Unauthorized;
                    message = exception.Message;
                    break;
            }
            var errorModel = new ErrorModel
            {
                Code = (int)code,
                Message = message,
                Detail = exception is ProjectException projectEx ? projectEx.Detail : null,
                Module = exception is ProjectException projectEx2 ? projectEx2.Module : null,
                ValidationError = exception is ProjectException projectEx3 ? projectEx3.ValidationError : null
            };
            var result = JsonConvert.SerializeObject(errorModel);
            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(result);
        }
    }
}
