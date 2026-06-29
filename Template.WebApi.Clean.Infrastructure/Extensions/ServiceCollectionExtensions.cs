using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace Template.WebApi.Clean.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRegisterService<TBaseInterface>(this IServiceCollection services, Assembly[]? assemblies = null, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            var baseType = typeof(TBaseInterface);
            assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
            var allTypes = assemblies.Where(a => !a.IsDynamic).SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t));
            foreach (var implType in allTypes)
            {
                var interfaces = implType.GetInterfaces()
                    .Where(i => baseType.IsAssignableFrom(i) && i != baseType);
                foreach (var iface in interfaces)
                    services.Add(new ServiceDescriptor(iface, implType, lifetime));
            }
            return services;
        }
    }
}
