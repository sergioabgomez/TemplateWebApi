namespace Template.WebApi.Clean.Infrastructure.Models
{
    public class ErrorResponse
    {
        public ErrorResponse() { }
        public ErrorResponse(ErrorModelValidation error) { Errors.Add(error); }
        public List<ErrorModelValidation> Errors { get; set; } = new();
    }
}
