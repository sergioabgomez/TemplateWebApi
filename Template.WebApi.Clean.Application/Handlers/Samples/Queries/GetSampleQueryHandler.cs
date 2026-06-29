using Cortex.Mediator.Queries;
using Microsoft.Extensions.Logging;
using Template.WebApi.Clean.Application.Models.Samples;
using Template.WebApi.Clean.Domain.Services;
namespace Template.WebApi.Clean.Application.Handlers.Samples.Queries
{
	public class GetSampleQueryHandler : IQueryHandler<GetSampleQuery, SampleResponse>
	{
		private readonly ILogger<GetSampleQueryHandler> logger;
		private readonly ISampleRepository repository;
		private readonly IDateTimeService dateTime;

		public GetSampleQueryHandler(
			ILogger<GetSampleQueryHandler> loggerFactory,
			ISampleRepository repository,
			IDateTimeService dateTime)
		{
			logger = loggerFactory;
			this.repository = repository;
			this.dateTime = dateTime;
		}

		public async Task<SampleResponse> Handle(GetSampleQuery query, CancellationToken ct)
		{
			logger.LogInformation("Getting sample: {Id} at {Now}", query.Id, dateTime.Now);

			Domain.Models.SampleEntity? entity = await repository.GetByIdAsync(query.Id, ct);
			if (entity is not null)
			{
				return new SampleResponse
				{
					Id = entity.Id,
					Name = entity.Name,
					CreatedAt = entity.CreatedAt
				};
			}

			return new SampleResponse { Id = query.Id, Name = "Sample Item", CreatedAt = dateTime.Now };
		}
	}
}
