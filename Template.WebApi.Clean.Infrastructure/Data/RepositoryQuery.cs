using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Template.WebApi.Clean.Infrastructure.Data.Contracts;
using Template.WebApi.Clean.Infrastructure.Data.Extensions;
using Template.WebApi.Clean.Infrastructure.Data.Models;

namespace Template.WebApi.Clean.Infrastructure.Data
{
	public class RepositoryQuery<TDbContext, TEntity> : IRepositoryQuery<TDbContext, TEntity>
		where TEntity : EntityBase
		where TDbContext : DbContext
	{
		private readonly bool tracking;
		private readonly DbContext context;
		private readonly DbSet<TEntity> dbentitySet;

		public RepositoryQuery(TDbContext context, bool tracking)
		{
			this.context = context;
			ValidateEntityInDbContext();

			this.tracking = tracking;
			dbentitySet = context.Set<TEntity>();
		}

		public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes)
		{
			return GetQuery(filter, orderBy, includes).ToList();
		}

		public virtual IEnumerable<TEntity> GetAll()
		{
			return dbentitySet.WithTracking(tracking).AsEnumerable().ToList();
		}

		public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await dbentitySet.WithTracking(tracking).ToListAsync();
		}

		public virtual async Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes)
		{
			return await GetQuery(filter, orderBy, includes).ToListAsync();
		}

		public async Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes)
		{
			IQueryable<TEntity> query = GetQuery(filter, orderBy, includes);

			var result = new PagedResult<TEntity>
			{
				CurrentPage = page,
				PageSize = pageSize,
				RowCount = query.Count()
			};

			var pageCount = (double)result.RowCount / pageSize;
			result.PageCount = (int)Math.Ceiling(pageCount);

			var skip = ( page - 1 ) * pageSize;
			result.Results = await query.Skip(skip).Take(pageSize).ToListAsync();
			result.PageSize = result.Results.Count;

			return result;
		}

		public virtual TEntity? GetById(params object[] keyValues)
		{
			TEntity? entity = dbentitySet.Find(keyValues);

			if (!tracking && entity is not null)
			{
				context.Entry(entity).State = EntityState.Detached;
			}

			return entity;
		}

		public virtual async Task<TEntity?> GetByIdAsync(params object[] keyValues)
		{
			TEntity? entity = await dbentitySet.FindAsync(keyValues);

			if (!tracking && entity is not null)
			{
				context.Entry(entity).State = EntityState.Detached;
			}

			return entity;
		}

		public virtual async Task<TEntity?> GetByIdAsync(CancellationToken cancellationToken, params object[] keyValues)
		{
			TEntity? entity = await dbentitySet.FindAsync(cancellationToken, keyValues);

			if (!tracking && entity is not null)
			{
				context.Entry(entity).State = EntityState.Detached;
			}

			return entity;
		}

		public virtual TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes)
		{
			IQueryable<TEntity> query = dbentitySet;

			foreach (Expression<Func<TEntity, object>> include in includes)
			{
				query = query.Include(include);
			}

			return filter is null
				? query.WithTracking(tracking).FirstOrDefault()
				: query.WithTracking(tracking).FirstOrDefault(filter);
		}

		public virtual async Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes)
		{
			IQueryable<TEntity> query = context.Set<TEntity>();

			foreach (Expression<Func<TEntity, object>> include in includes)
			{
				query = query.Include(include);
			}

			return filter is null
				? await query.WithTracking(tracking).FirstOrDefaultAsync()
				: await query.WithTracking(tracking).FirstOrDefaultAsync(filter);
		}

		public virtual async Task<TEntity?> GetLastOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes)
		{
			IQueryable<TEntity> query = context.Set<TEntity>();

			foreach (Expression<Func<TEntity, object>> include in includes)
			{
				query = query.Include(include);
			}

			if (orderBy is not null)
			{
				query = orderBy(query);
			}

			if (filter is null)
			{
				return await query.WithTracking(tracking).LastOrDefaultAsync();
			}

			return await query.WithTracking(tracking).LastOrDefaultAsync(filter);
		}

		public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
		{
			IQueryable<TEntity> query = dbentitySet;

			if (filter is not null)
			{
				query = query.Where(filter);
			}

			if (orderBy is not null)
			{
				query = orderBy(query);
			}

			return query.WithTracking(tracking);
		}

		public virtual IQueryable<TEntity> Queryable()
		{
			return dbentitySet.WithTracking(tracking);
		}

		private void ValidateEntityInDbContext()
		{
			if (context.Model.FindEntityType(typeof(TEntity)) is null)
			{
				throw new InvalidOperationException($"Entity type {typeof(TEntity)} is not registered in {typeof(TDbContext).Name}");
			}
		}

		private IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes)
		{
			IQueryable<TEntity> query = dbentitySet;

			foreach (Expression<Func<TEntity, object>> include in includes)
			{
				query = query.Include(include);
			}

			if (filter is not null)
			{
				query = query.Where(filter);
			}

			if (orderBy is not null)
			{
				query = orderBy(query);
			}

			return query.AsNoTracking();
		}
	}
}
