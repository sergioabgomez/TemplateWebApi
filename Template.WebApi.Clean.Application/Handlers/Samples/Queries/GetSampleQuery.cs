using Cortex.Mediator.Queries;
using Template.WebApi.Clean.Application.Models.Samples;
namespace Template.WebApi.Clean.Application.Handlers.Samples.Queries
{
    public class GetSampleQuery : IQuery<SampleResponse>
    {
        public Guid Id { get; }
        public GetSampleQuery(Guid id) { Id = id; }
    }
}
