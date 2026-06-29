namespace Template.WebApi.Clean.Installers.Contracts
{
	public interface IInstallerApplicationBuilder
	{
		public void InstallApplication(IApplicationBuilder app, IConfiguration configuration);
	}

	public interface IInstallerServiceCollection
	{
		public void InstallServices(IServiceCollection services, IConfiguration configuration);
	}
}
