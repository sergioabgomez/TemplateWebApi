using Cortex.Mediator.Commands;
using Microsoft.Extensions.Logging;
using Template.WebApi.Clean.Application.Models.Samples;
using Template.WebApi.Clean.Domain.Services;
namespace Template.WebApi.Clean.Application.Handlers.Samples.Commands
{
	public class CreateSampleCommandHandler : ICommandHandler<CreateSampleCommand, CreateSampleResponse>
	{
		private readonly ILogger<CreateSampleCommandHandler> logger;
		private readonly ISampleRepository repository;
		private readonly IDateTimeService dateTime;

		public CreateSampleCommandHandler(
			ILogger<CreateSampleCommandHandler> loggerFactory,
			ISampleRepository repository,
			IDateTimeService dateTime)
		{
			logger = loggerFactory;
			this.repository = repository;
			this.dateTime = dateTime;
		}

		public async Task<CreateSampleResponse> Handle(CreateSampleCommand cmd, CancellationToken ct)
		{
			logger.LogInformation("Creating sample: {Name} at {Now}", cmd.Name, dateTime.Now);

			Domain.Models.SampleEntity entity = await repository.AddAsync(cmd.Name, cmd.Description, ct);

			return new CreateSampleResponse
			{
				Id = entity.Id,
				Name = entity.Name,
				CreatedAt = entity.CreatedAt
			};
		}
	}
}
