namespace Template.WebApi.Clean.Routes
{
	/// <summary>
	/// API route constants for versioning, base path, and content types.
	/// </summary>
	public class ApiRoutes
	{
		/// <summary>
		/// API version 1.0
		/// </summary>
		public const string VersionOne = "1";

		/// <summary>
		/// API version 2.0
		/// </summary>
		public const string VersionTwo = "2";

		/// <summary>
		/// Base route pattern: api/v{version:apiVersion}/[controller]
		/// </summary>
		public const string Base = "api/v{version:apiVersion}/[controller]";

		/// <summary>
		/// Default response content type: application/json
		/// </summary>
		public const string Produces = "application/json";

		/// <summary>
		/// Default request content type: application/json
		/// </summary>
		public const string Consumes = "application/json";
	}
}
