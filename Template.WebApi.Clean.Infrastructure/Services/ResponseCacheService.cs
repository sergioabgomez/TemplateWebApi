using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Template.WebApi.Clean.Domain.Services;

namespace Template.WebApi.Clean.Infrastructure.Services
{
	public class ResponseCacheService : IResponseCacheService
	{
		private readonly IDistributedCache distributedCache;

		public ResponseCacheService(IDistributedCache distributedCache)
		{
			this.distributedCache = distributedCache;
		}

		public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
		{
			if (response == null)
			{
				return;
			}

			var serializedResponse = JsonSerializer.Serialize(response);

			await distributedCache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = timeToLive
			});
		}

		public async Task<string?> GetCachedResponseAsync(string cacheKey)
		{
			var cachedResponse = await distributedCache.GetStringAsync(cacheKey);
			return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
		}
	}
}
