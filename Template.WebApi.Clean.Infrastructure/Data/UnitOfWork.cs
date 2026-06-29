using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Template.WebApi.Clean.Infrastructure.Data.Contracts;
using Template.WebApi.Clean.Infrastructure.Data.Models;

namespace Template.WebApi.Clean.Infrastructure.Data
{
	public class UnitOfWork<TDbContext, TEntity> : IUnitOfWork<TDbContext, TEntity>, IAsyncDisposable
		where TDbContext : DbContext
		where TEntity : EntityBase
	{
		private bool disposed;
		private readonly bool tracking;
		private TDbContext? dbContext;
		private readonly IDbContextFactory<TDbContext> dbFactory;

		public UnitOfWork(IDbContextFactory<TDbContext> dbFactory, bool tracking)
		{
			this.dbFactory = dbFactory;
			this.tracking = tracking;
		}

		private TDbContext Context => dbContext ??= dbFactory.CreateDbContext();

		public IRepositoryCommand<TDbContext, TEntity> RepositoryCommand =>
			field ??= new RepositoryCommand<TDbContext, TEntity>(Context, tracking);

		public IRepositoryQuery<TDbContext, TEntity> RepositoryQuery =>
			field ??= new RepositoryQuery<TDbContext, TEntity>(Context, tracking);

		public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			IExecutionStrategy strategy = Context.Database.CreateExecutionStrategy();

			await strategy.ExecuteAsync(async () =>
			{
				await using IDbContextTransaction transaction = await Context.Database.BeginTransactionAsync(cancellationToken);
				try
				{
					await Context.SaveChangesAsync(cancellationToken);
					await transaction.CommitAsync(cancellationToken);
				}
				catch
				{
					await transaction.RollbackAsync(cancellationToken);
					throw;
				}
			});
		}

		public async ValueTask DisposeAsync()
		{
			if (!disposed)
			{
				if (dbContext is not null)
				{
					await dbContext.DisposeAsync();
				}

				disposed = true;
			}
		}
	}
}
