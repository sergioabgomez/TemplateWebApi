using System.ComponentModel.DataAnnotations;

namespace Template.WebApi.Clean.Infrastructure.Data.Models
{
	public class SampleEfEntity : EntityBaseWithId
	{
		[MaxLength(200)]
		public string Name { get; set; } = string.Empty;

		[MaxLength(1000)]
		public string? Description { get; set; }

		public DateTime CreatedAt { get; set; }
	}
}
