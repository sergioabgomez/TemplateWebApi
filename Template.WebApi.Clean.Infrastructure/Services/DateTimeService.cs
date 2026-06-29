using Template.WebApi.Clean.Domain.Services;

namespace Template.WebApi.Clean.Infrastructure.Services
{
	public class DateTimeService : IDateTimeService
	{
		public DateTime Now => DateTime.UtcNow;
	}
}
