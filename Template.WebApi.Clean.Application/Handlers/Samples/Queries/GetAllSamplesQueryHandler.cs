using Cortex.Mediator.Queries;
using Microsoft.Extensions.Logging;
using Template.WebApi.Clean.Application.Models.Samples;
using Template.WebApi.Clean.Domain.Services;
namespace Template.WebApi.Clean.Application.Handlers.Samples.Queries
{
	public class GetAllSamplesQueryHandler : IQueryHandler<GetAllSamplesQuery, List<SampleResponse>>
	{
		private readonly ILogger<GetAllSamplesQueryHandler> logger;
		private readonly ISampleRepository repository;
		private readonly IDateTimeService dateTime;

		public GetAllSamplesQueryHandler(
			ILogger<GetAllSamplesQueryHandler> loggerFactory,
			ISampleRepository repository,
			IDateTimeService dateTime)
		{
			logger = loggerFactory;
			this.repository = repository;
			this.dateTime = dateTime;
		}

		public async Task<List<SampleResponse>> Handle(GetAllSamplesQuery query, CancellationToken ct)
		{
			logger.LogInformation("Getting all samples at {Now}", dateTime.Now);

			List<Domain.Models.SampleEntity> entities = await repository.GetAllAsync(ct);

			return entities.Select(e => new SampleResponse
			{
				Id = e.Id,
				Name = e.Name,
				CreatedAt = e.CreatedAt
			}).ToList();
		}
	}
}
