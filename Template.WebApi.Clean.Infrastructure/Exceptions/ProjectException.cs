namespace Template.WebApi.Clean.Infrastructure.Exceptions
{
	public class ProjectException : ApplicationException
	{
		public bool WithLogError { get; set; } = false;
		public string? Module { get; set; }
		public string? Detail { get; set; }
		public ProjectException() { }
		public ProjectException(string message) : base(message) { }
		public ProjectException(string message, Exception inner) : base(message, inner) { }
	}
}
