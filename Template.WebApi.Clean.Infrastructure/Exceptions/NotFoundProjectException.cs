namespace Template.WebApi.Clean.Infrastructure.Exceptions
{
    public class NotFoundProjectException : ProjectException
    {
        public NotFoundProjectException() { }
        public NotFoundProjectException(string message) : base(message) { }
        public NotFoundProjectException(string message, Exception inner) : base(message, inner) { }
    }
}
