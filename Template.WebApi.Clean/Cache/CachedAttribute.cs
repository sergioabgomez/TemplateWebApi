using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Template.WebApi.Clean.Application.Configurations;
using Template.WebApi.Clean.Domain.Services;

namespace Template.WebApi.Clean.Cache
{
	/// <summary>
	/// Caches GET action responses using an <see cref="IResponseCacheService"/>.
	/// Skips non-GET requests and respects <see cref="RedisCacheSettings.Enabled"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class CachedAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int timeToLiveSeconds;
		private const string MethodGet = "GET";
		private const string ContentType = "application/json";

		/// <summary>
		/// CachedAttribute constructor
		/// </summary>
		/// <param name="timeToLiveSeconds">Seconds the response stays in cache.</param>
		public CachedAttribute(int timeToLiveSeconds)
		{
			this.timeToLiveSeconds = timeToLiveSeconds;
		}

		/// <summary>
		/// Executes the cache check before the action and stores the response after.
		/// </summary>
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (context.HttpContext.Request.Method != MethodGet)
			{
				await next();
				return;
			}

			RedisCacheSettings cacheSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();

			if (!cacheSettings.Enabled)
			{
				await next();
				return;
			}

			IResponseCacheService cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

			var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
			var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

			if (!string.IsNullOrEmpty(cachedResponse))
			{
				var contentResult = new ContentResult
				{
					Content = cachedResponse,
					ContentType = ContentType,
					StatusCode = (int)HttpStatusCode.OK
				};

				context.Result = contentResult;
				return;
			}

			ActionExecutedContext executedContext = await next();

			if (executedContext.Result is OkObjectResult { Value: not null } okObjectResult)
			{
				await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(timeToLiveSeconds));
			}
		}

		private static string GenerateCacheKeyFromRequest(HttpRequest request)
		{
			var keyBuilder = new StringBuilder();

			keyBuilder.Append($"{request.Path}");

			foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
			{
				keyBuilder.Append($"|{key}-{value}");
			}

			return keyBuilder.ToString();
		}
	}
}
