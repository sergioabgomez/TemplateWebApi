using Template.WebApi.Clean.Installers.Contracts;

namespace Template.WebApi.Clean.Installers.Extensions
{
	public static class InstallerExtensions
	{
		public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
		{
			var installers = typeof(Program).Assembly.ExportedTypes
				.Where(x => typeof(IInstallerServiceCollection).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
				.Select(Activator.CreateInstance).Cast<IInstallerServiceCollection>().ToList();
			installers.ForEach(installer => installer.InstallServices(services, configuration));
		}

		public static void InstallApplicationInAssembly(this IApplicationBuilder app, IConfiguration configuration)
		{
			var installers = typeof(Program).Assembly.ExportedTypes
				.Where(x => typeof(IInstallerApplicationBuilder).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
				.Select(Activator.CreateInstance).Cast<IInstallerApplicationBuilder>().ToList();
			installers.ForEach(installer => installer.InstallApplication(app, configuration));
		}
	}
}
