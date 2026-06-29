using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
namespace Template.WebApi.Clean.Infrastructure.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddRegisterService<TBaseInterface>(this IServiceCollection services, Assembly[]? assemblies = null, ServiceLifetime lifetime = ServiceLifetime.Scoped)
		{
			Type baseType = typeof(TBaseInterface);
			assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
			IEnumerable<Type> allTypes = assemblies.Where(a => !a.IsDynamic).SelectMany(a => a.GetTypes())
				.Where(t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t));
			foreach (Type? implType in allTypes)
			{
				IEnumerable<Type> interfaces = implType.GetInterfaces()
					.Where(i => baseType.IsAssignableFrom(i) && i != baseType);
				foreach (Type? iface in interfaces)
				{
					services.Add(new ServiceDescriptor(iface, implType, lifetime));
				}
			}
			return services;
		}
	}
}
