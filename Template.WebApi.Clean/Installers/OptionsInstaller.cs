using Template.WebApi.Clean.Installers.Contracts;

namespace Template.WebApi.Clean.Installers
{
	public class OptionsInstaller : IInstallerServiceCollection
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddOptions();
		}
	}
}
