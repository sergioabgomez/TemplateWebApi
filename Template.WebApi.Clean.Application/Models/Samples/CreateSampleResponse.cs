namespace Template.WebApi.Clean.Application.Models.Samples
{
    public class CreateSampleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
