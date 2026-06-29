using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Template.WebApi.Clean.Infrastructure.Data.Contracts;
using Template.WebApi.Clean.Infrastructure.Data.Models;

namespace Template.WebApi.Clean.Infrastructure.Data
{
	public class RepositoryCommand<TDbContext, TEntity> : IRepositoryCommand<TDbContext, TEntity>
		where TEntity : EntityBase
		where TDbContext : DbContext
	{
		private readonly bool tracking;
		private readonly DbContext context;
		private readonly DbSet<TEntity> dbentitySet;

		public RepositoryCommand(TDbContext context, bool tracking)
		{
			this.context = context;
			ValidateEntityInDbContext();

			this.tracking = tracking;
			dbentitySet = context.Set<TEntity>();
		}

		public TEntity Create(TEntity entity)
		{
			EntityEntry<TEntity> newEntity = dbentitySet.Add(entity);

			return newEntity.Entity;
		}

		public IEnumerable<TEntity> CreateRange(IEnumerable<TEntity> entities)
		{
			dbentitySet.AddRange(entities);

			return entities;
		}

		public void Delete(params object[] keyValues)
		{
			TEntity? entityToDelete = dbentitySet.Find(keyValues);

			if (entityToDelete is null)
			{
				var name = typeof(TEntity).Name;

				throw new InvalidOperationException($"Entity '{name}' with ID {string.Join(",", keyValues)} not found.");
			}

			Delete(entityToDelete);
		}

		public void Delete(TEntity entityToDelete)
		{
			if (context.Entry(entityToDelete).State == EntityState.Detached)
			{
				dbentitySet.Attach(entityToDelete);
			}

			dbentitySet.Remove(entityToDelete);
		}

		public TEntity Update(TEntity entityToUpdate)
		{
			EntityEntry<TEntity> entityUpdated = dbentitySet.Attach(entityToUpdate);

			context.Entry(entityToUpdate).State = EntityState.Modified;

			return entityUpdated.Entity;
		}

		public async Task<TEntity> UpdateAsync(TEntity entityToUpdate)
		{
			EntityEntry<TEntity> entityUpdated = dbentitySet.Attach(entityToUpdate);

			context.Entry(entityToUpdate).State = EntityState.Modified;

			await Task.CompletedTask;

			return entityUpdated.Entity;
		}

		private void ValidateEntityInDbContext()
		{
			if (context.Model.FindEntityType(typeof(TEntity)) is null)
			{
				throw new InvalidOperationException($"Entity type {typeof(TEntity)} is not registered in {typeof(TDbContext).Name}");
			}
		}
	}
}
