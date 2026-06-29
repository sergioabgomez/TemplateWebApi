using Cortex.Mediator.Queries;
using Microsoft.Extensions.Logging;
using Template.WebApi.Clean.Application.Models.Samples;
namespace Template.WebApi.Clean.Application.Handlers.Samples.Queries
{
    public class GetSampleQueryHandler : IQueryHandler<GetSampleQuery, SampleResponse>
    {
        private readonly ILogger<GetSampleQueryHandler> _logger;
        public GetSampleQueryHandler(ILogger<GetSampleQueryHandler> logger) { _logger = logger; }
        public async Task<SampleResponse> Handle(GetSampleQuery query, CancellationToken ct)
        {
            _logger.LogInformation("Getting sample: {Id}", query.Id);
            await Task.CompletedTask;
            return new SampleResponse { Id = query.Id, Name = "Sample Item", CreatedAt = DateTime.UtcNow };
        }
    }
}
