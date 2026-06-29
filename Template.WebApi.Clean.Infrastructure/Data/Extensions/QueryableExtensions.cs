using Microsoft.EntityFrameworkCore;

namespace Template.WebApi.Clean.Infrastructure.Data.Extensions
{
	public static class QueryableExtensions
	{
		public static IQueryable<TEntity> WithTracking<TEntity>(this IQueryable<TEntity> source, bool tracking)
			where TEntity : class
		{
			return tracking ? source.AsTracking() : source.AsNoTracking();
		}
	}
}
