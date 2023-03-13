namespace Microsoft.Extensions.DependencyInjection;
using System.Reflection;

public static class TypesAssignableFromServiceCollectionExtensions
{
    /// <summary>
    /// Adds all types assignable from TInterface in the assembly to the service collection.
    /// </summary>
    /// <typeparam name="TInterface">The interface to scan for.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="assembly">The assembly to scan. If null, the calling assembly is used.</param>
    public static IServiceCollection AddScopedTypesAssignableFrom<TInterface>(this IServiceCollection services, Assembly? assembly = null)
        => services.AddTypesAssignableFrom<TInterface>(ServiceLifetime.Scoped, assembly);

    /// <summary>
    /// Adds all types assignable from TInterface in the assembly to the service collection.
    /// </summary>
    /// <typeparam name="TInterface">The interface to scan for.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="assembly">The assembly to scan. If null, the calling assembly is used.</param>
    public static IServiceCollection AddSingletonTypesAssignableFrom<TInterface>(this IServiceCollection services, Assembly? assembly = null)
        => services.AddTypesAssignableFrom<TInterface>(ServiceLifetime.Singleton, assembly);

    /// <summary>
    /// Adds all types assignable from TInterface in the assembly to the service collection.
    /// </summary>
    /// <typeparam name="TInterface">The interface to scan for.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="assembly">The assembly to scan. If null, the calling assembly is used.</param>
    public static IServiceCollection AddTransientTypesAssignableFrom<TInterface>(this IServiceCollection services, Assembly? assembly = null)
        => services.AddTypesAssignableFrom<TInterface>(ServiceLifetime.Transient, assembly);


    /// <summary>
    /// Adds all types assignable from TInterface in the assembly to the service collection.
    /// </summary>
    /// <typeparam name="TInterface">The interface to scan for.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <param name="lifetime">The lifetime of the services.</param>
    /// <param name="assembly">The assembly to scan. If null, the calling assembly is used.</param>
    public static IServiceCollection AddTypesAssignableFrom<TInterface>(this IServiceCollection services, ServiceLifetime lifetime, Assembly? assembly = null)
    {
        var types = (assembly ?? Assembly.GetExecutingAssembly()).GetTypes();
        foreach (var type in types.Where(t => !t.IsAbstract && typeof(TInterface).IsAssignableFrom(t)))
        {
            services.Add(new ServiceDescriptor(typeof(TInterface), type, lifetime));
        }

        return services;
    }
}