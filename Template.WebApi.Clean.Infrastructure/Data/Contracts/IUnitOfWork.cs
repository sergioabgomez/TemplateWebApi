using Microsoft.EntityFrameworkCore;
using Template.WebApi.Clean.Infrastructure.Data.Models;

namespace Template.WebApi.Clean.Infrastructure.Data.Contracts
{
	public interface IUnitOfWork<TDbContext, TEntity> : IAsyncDisposable
		where TDbContext : DbContext
		where TEntity : EntityBase
	{
		public IRepositoryCommand<TDbContext, TEntity> RepositoryCommand { get; }
		public IRepositoryQuery<TDbContext, TEntity> RepositoryQuery { get; }

		public Task SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
