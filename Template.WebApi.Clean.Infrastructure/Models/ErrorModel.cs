using Newtonsoft.Json;
using Template.WebApi.Clean.Infrastructure.Bootstrappers;
namespace Template.WebApi.Clean.Infrastructure.Models
{
    public class ErrorModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Detail { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Module { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<ValidationError> ValidationError { get; set; }
    }
}
