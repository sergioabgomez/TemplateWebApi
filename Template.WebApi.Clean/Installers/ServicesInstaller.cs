using System.Reflection;
using Template.WebApi.Clean.Application;
using Template.WebApi.Clean.Domain.Services;
using Template.WebApi.Clean.Infrastructure;
using Template.WebApi.Clean.Infrastructure.Extensions;
using Template.WebApi.Clean.Infrastructure.Services;
using Template.WebApi.Clean.Installers.Contracts;

namespace Template.WebApi.Clean.Installers
{
	public class ServicesInstaller : IInstallerServiceCollection
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			// Auto-register infrastructure services via marker interfaces
			Assembly[] assemblies = new[] { typeof(DummyApplication), typeof(DummyInfrastructure) }
				.Select(a => a.Assembly).ToArray();
			services.AddRegisterService<IServiceScoped>(assemblies, ServiceLifetime.Scoped);
			services.AddRegisterService<IServiceTransient>(assemblies, ServiceLifetime.Transient);
			services.AddRegisterService<IServiceSingleton>(assemblies, ServiceLifetime.Singleton);

			// Manual registrations for domain abstraction / infrastructure implementation pairs
			services.AddSingleton<IDateTimeService, DateTimeService>();
			services.AddScoped<ISampleRepository, SampleRepository>();
		}
	}
}
