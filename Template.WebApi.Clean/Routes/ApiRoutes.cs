namespace Template.WebApi.Clean.Routes
{
	public class ApiRoutes
	{
		public const string VersionOne = "1";
		public const string VersionTwo = "2";
		public const string Base = "api/v{version:apiVersion}/[controller]";
		public const string Produces = "application/json";
		public const string Consumes = "application/json";
	}
}
