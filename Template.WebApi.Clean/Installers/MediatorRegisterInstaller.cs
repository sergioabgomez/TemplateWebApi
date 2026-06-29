using Cortex.Mediator.DependencyInjection;
using Template.WebApi.Clean.Application;
using Template.WebApi.Clean.Infrastructure;
using Template.WebApi.Clean.Installers.Contracts;

namespace Template.WebApi.Clean.Installers
{
    public class MediatorRegisterInstaller : IInstallerServiceCollection
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCortexMediator(
                new[] { typeof(DummyApplication), typeof(DummyInfrastructure) }
            );
        }
    }
}
