using Template.WebApi.Clean.Domain.Models;

namespace Template.WebApi.Clean.Domain.Services
{
	public interface ISampleRepository
	{
		public Task<List<SampleEntity>> GetAllAsync(CancellationToken ct);
		public Task<SampleEntity?> GetByIdAsync(Guid id, CancellationToken ct);
		public Task<SampleEntity> AddAsync(string name, string? description, CancellationToken ct);
	}
}
