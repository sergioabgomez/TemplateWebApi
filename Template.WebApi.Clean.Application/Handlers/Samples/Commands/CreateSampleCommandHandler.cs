using Cortex.Mediator.Commands;
using Microsoft.Extensions.Logging;
using Template.WebApi.Clean.Application.Models.Samples;
namespace Template.WebApi.Clean.Application.Handlers.Samples.Commands
{
    public class CreateSampleCommandHandler : ICommandHandler<CreateSampleCommand, CreateSampleResponse>
    {
        private readonly ILogger<CreateSampleCommandHandler> _logger;
        public CreateSampleCommandHandler(ILogger<CreateSampleCommandHandler> logger) { _logger = logger; }
        public async Task<CreateSampleResponse> Handle(CreateSampleCommand cmd, CancellationToken ct)
        {
            _logger.LogInformation("Creating sample: {Name}", cmd.Name);
            await Task.CompletedTask;
            return new CreateSampleResponse { Id = Guid.NewGuid(), Name = cmd.Name, CreatedAt = DateTime.UtcNow };
        }
    }
}
