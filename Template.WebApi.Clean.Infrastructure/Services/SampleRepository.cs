using System.Collections.Concurrent;
using Template.WebApi.Clean.Domain.Models;
using Template.WebApi.Clean.Domain.Services;

namespace Template.WebApi.Clean.Infrastructure.Services
{
	public class SampleRepository : ISampleRepository
	{
		private readonly ConcurrentDictionary<Guid, SampleEntity> samples = new();
		private readonly IDateTimeService dateTime;

		public SampleRepository(IDateTimeService dateTime)
		{
			this.dateTime = dateTime;
			Seed();
		}

		private void Seed()
		{
			var item = new SampleEntity
			{
				Id = Guid.NewGuid(),
				Name = "Default Sample",
				Description = "Preloaded sample item",
				CreatedAt = dateTime.Now
			};
			samples.TryAdd(item.Id, item);
		}

		public Task<List<SampleEntity>> GetAllAsync(CancellationToken ct)
		{
			return Task.FromResult(samples.Values.ToList());
		}

		public Task<SampleEntity?> GetByIdAsync(Guid id, CancellationToken ct)
		{
			samples.TryGetValue(id, out SampleEntity? entity);
			return Task.FromResult(entity);
		}

		public Task<SampleEntity> AddAsync(string name, string? description, CancellationToken ct)
		{
			var entity = new SampleEntity
			{
				Id = Guid.NewGuid(),
				Name = name,
				Description = description,
				CreatedAt = dateTime.Now
			};
			samples.TryAdd(entity.Id, entity);
			return Task.FromResult(entity);
		}
	}
}
