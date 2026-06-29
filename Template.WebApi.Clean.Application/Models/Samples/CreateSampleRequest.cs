namespace Template.WebApi.Clean.Application.Models.Samples
{
	public class CreateSampleRequest
	{
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
	}
}
