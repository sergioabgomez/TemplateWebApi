using System.Net;
namespace Template.WebApi.Clean.Infrastructure.Exceptions
{
	public class CustomExceptionProjectException : ProjectException
	{
		public HttpStatusCode StatusCode { get; }
		public CustomExceptionProjectException(HttpStatusCode statusCode) { StatusCode = statusCode; }
		public CustomExceptionProjectException(HttpStatusCode statusCode, string message) : base(message) { StatusCode = statusCode; }
		public CustomExceptionProjectException(HttpStatusCode statusCode, string message, Exception inner) : base(message, inner) { StatusCode = statusCode; }
	}
}
