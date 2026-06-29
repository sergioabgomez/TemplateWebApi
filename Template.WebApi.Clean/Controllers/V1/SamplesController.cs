using Asp.Versioning;
using Cortex.Mediator;
using Microsoft.AspNetCore.Mvc;
using Template.WebApi.Clean.Application.Handlers.Samples.Commands;
using Template.WebApi.Clean.Application.Handlers.Samples.Queries;
using Template.WebApi.Clean.Application.Models.Samples;
using Template.WebApi.Clean.Cache;
using Template.WebApi.Clean.Routes;

namespace Template.WebApi.Clean.Controllers.V1
{
	/// <summary>
	/// Sample endpoints for demonstration and template validation.
	/// </summary>
	[ApiVersion(ApiRoutes.VersionOne)]
	[Route(ApiRoutes.Base)]
	[Produces(ApiRoutes.Produces)]
	[Consumes(ApiRoutes.Consumes)]
	[ApiController]
	public class SamplesController : ControllerBase
	{
		private readonly IMediator mediator;

		/// <summary>
		/// SamplesController constructor
		/// </summary>
		/// <param name="mediatorService">Cortex.Mediator instance for CQRS dispatch</param>
		public SamplesController(IMediator mediatorService)
		{
			mediator = mediatorService;
		}

		/// <summary>
		/// Get all samples.
		/// </summary>
		/// <returns>List of <see cref="SampleResponse"/>.</returns>
		[HttpGet]
		[MapToApiVersion(ApiRoutes.VersionOne)]
		[Cached(CacheTimeHelper.SixHundredSeconds)]
		[ProducesResponseType(typeof(List<SampleResponse>), 200)]
		[ProducesResponseType(typeof(ProblemDetails), 500)]
		public async Task<IActionResult> GetAllSamplesAsync()
		{
			List<SampleResponse> result = await mediator.SendQueryAsync<GetAllSamplesQuery, List<SampleResponse>>(new GetAllSamplesQuery());
			return Ok(result);
		}

		/// <summary>
		/// Get a sample by its identifier.
		/// </summary>
		/// <param name="id">Sample unique identifier.</param>
		/// <returns>A <see cref="SampleResponse"/>.</returns>
		[HttpGet("{id}")]
		[MapToApiVersion(ApiRoutes.VersionOne)]
		[Cached(CacheTimeHelper.SixHundredSeconds)]
		[ProducesResponseType(typeof(SampleResponse), 200)]
		[ProducesResponseType(typeof(ProblemDetails), 500)]
		public async Task<IActionResult> GetSampleAsync(Guid id)
		{
			SampleResponse result = await mediator.SendQueryAsync<GetSampleQuery, SampleResponse>(new GetSampleQuery(id));
			return Ok(result);
		}

		/// <summary>
		/// Create a new sample.
		/// </summary>
		/// <param name="request">Sample creation data.</param>
		/// <returns>The created <see cref="CreateSampleResponse"/> with a 201 status.</returns>
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
