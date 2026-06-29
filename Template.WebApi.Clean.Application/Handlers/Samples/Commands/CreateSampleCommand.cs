using Cortex.Mediator.Commands;
using Template.WebApi.Clean.Application.Models.Samples;
namespace Template.WebApi.Clean.Application.Handlers.Samples.Commands
{
	public class CreateSampleCommand : ICommand<CreateSampleResponse>
	{
		public string Name { get; }
		public string? Description { get; }
		public CreateSampleCommand(CreateSampleRequest request)
		{
			Name = request.Name;
			Description = request.Description;
		}
	}
}
