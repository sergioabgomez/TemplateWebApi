using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Template.WebApi.Clean.Infrastructure.Data.Models;

namespace Template.WebApi.Clean.Infrastructure.Data.Contracts
{
	public interface IRepositoryQuery<TDbContext, TEntity>
		where TEntity : EntityBase
		where TDbContext : DbContext
	{
		public IQueryable<TEntity> Queryable();

		public IEnumerable<TEntity> GetAll();

		public Task<IEnumerable<TEntity>> GetAllAsync();

		public IList<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes);

		public Task<IList<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes);

		public Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes);

		public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null);

		public TEntity? GetById(params object[] keyValues);

		public Task<TEntity?> GetByIdAsync(params object[] keyValues);

		public Task<TEntity?> GetByIdAsync(CancellationToken cancellationToken, params object[] keyValues);

		public TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes);

		public Task<TEntity?> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes);

		public Task<TEntity?> GetLastOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, params Expression<Func<TEntity, object>>[] includes);
	}
}
