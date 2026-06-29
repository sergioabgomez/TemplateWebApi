using Asp.Versioning;
using Cortex.Mediator;
using Microsoft.AspNetCore.Mvc;
using Template.WebApi.Clean.Application.Handlers.Samples.Commands;
using Template.WebApi.Clean.Application.Handlers.Samples.Queries;
using Template.WebApi.Clean.Application.Models.Samples;
using Template.WebApi.Clean.Routes;

namespace Template.WebApi.Clean.Controllers
{
	[ApiVersion(ApiRoutes.VersionOne)]
	[Route(ApiRoutes.Base)]
	[Produces(ApiRoutes.Produces)]
	[Consumes(ApiRoutes.Consumes)]
	[ApiController]
	public class SamplesController : ControllerBase
	{
		private readonly IMediator mediator;

		public SamplesController(IMediator mediatorService)
		{
			mediator = mediatorService;
		}

		[HttpGet]
		[MapToApiVersion(ApiRoutes.VersionOne)]
		[ProducesResponseType(typeof(List<SampleResponse>), 200)]
		[ProducesResponseType(typeof(ProblemDetails), 500)]
		public async Task<IActionResult> GetAllSamplesAsync()
		{
			List<SampleResponse> result = await mediator.SendQueryAsync<GetAllSamplesQuery, List<SampleResponse>>(new GetAllSamplesQuery());
			return Ok(result);
		}

		[HttpGet("{id}")]
		[MapToApiVersion(ApiRoutes.VersionOne)]
		[ProducesResponseType(typeof(SampleResponse), 200)]
		[ProducesResponseType(typeof(ProblemDetails), 500)]
		public async Task<IActionResult> GetSampleAsync(Guid id)
		{
			SampleResponse result = await mediator.SendQueryAsync<GetSampleQuery, SampleResponse>(new GetSampleQuery(id));
			return Ok(result);
		}

		[HttpPost]
		[MapToApiVersion(ApiRoutes.VersionOne)]
		[ProducesResponseType(typeof(CreateSampleResponse), 201)]
		[ProducesResponseType(typeof(ProblemDetails), 400)]
		[ProducesResponseType(typeof(ProblemDetails), 500)]
		public async Task<IActionResult> CreateSampleAsync([FromBody] CreateSampleRequest request)
		{
			CreateSampleResponse result = await mediator.SendCommandAsync<CreateSampleCommand, CreateSampleResponse>(new CreateSampleCommand(request));
			return CreatedAtAction(nameof(GetSampleAsync), new { id = result.Id }, result);
		}
	}
}
