using Microsoft.EntityFrameworkCore;
using Template.WebApi.Clean.Infrastructure.Data.Models;

namespace Template.WebApi.Clean.Infrastructure.Data.Contracts
{
	public interface IRepositoryCommand<TDbContext, TEntity>
		where TEntity : EntityBase
		where TDbContext : DbContext
	{
		public TEntity Create(TEntity entity);

		public IEnumerable<TEntity> CreateRange(IEnumerable<TEntity> entities);

		public void Delete(params object[] keyValues);

		public void Delete(TEntity entityToDelete);

		public TEntity Update(TEntity entityToUpdate);

		public Task<TEntity> UpdateAsync(TEntity entityToUpdate);
	}
}
