using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Template.WebApi.Clean.Infrastructure.Data.Models
{
	public interface IDataModelBase { }

	public abstract class EntityBase : IDataModelBase { }

	public abstract class EntityBaseWithId : EntityBase
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
	}

	public abstract class AuditEntityBase : EntityBase
	{
		public DateTime? CreatedAt { get; set; }
		public string? CreatedByName { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string? UpdatedByName { get; set; }
	}
}
