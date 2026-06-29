namespace Template.WebApi.Clean.Infrastructure.Data.Models
{
	public class PagedResult<T> : PagedResultBase where T : class
	{
		public IList<T> Results { get; set; } = new List<T>();
	}
}
