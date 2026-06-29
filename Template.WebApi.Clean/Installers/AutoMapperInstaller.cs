using Mapster;
using Template.WebApi.Clean.Application.AutoMapper;
using Template.WebApi.Clean.Installers.Contracts;

namespace Template.WebApi.Clean.Installers
{
    public class AutoMapperInstaller : IInstallerServiceCollection
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMapster();
            MapsterConfig.Configure();
        }
    }
}
