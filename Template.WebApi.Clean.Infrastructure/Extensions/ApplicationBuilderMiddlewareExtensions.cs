using Template.WebApi.Clean.Infrastructure.Middlewares;
using Microsoft.AspNetCore.Builder;
namespace Template.WebApi.Clean.Infrastructure.Extensions
{
    public static class ApplicationBuilderMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
            => builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
