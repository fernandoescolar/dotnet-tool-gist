global using Framework;
namespace Framework;
using System.Reflection;

public interface IEndpoint
{
    RouteHandlerBuilder Configure(IEndpointRouteBuilder builder);
}
public abstract record Endpoint(string[] Verbs, string Path) : IEndpoint
{
    public Endpoint(string verb, string path) : this(new[] { verb }, path) { }

    public RouteHandlerBuilder Configure(IEndpointRouteBuilder builder)
        => Configure(builder.MapMethods(Path, Verbs, InternalHandler));

    protected virtual RouteHandlerBuilder Configure(RouteHandlerBuilder builder)
        => builder;

    protected abstract IResult Handle();

    private IResult InternalHandler()
        => Handle();
}
public abstract record AsyncEndpoint(string[] Verbs, string Path) : IEndpoint
{
    public AsyncEndpoint(string verb, string path) : this(new[] { verb }, path) { }

    public RouteHandlerBuilder Configure(IEndpointRouteBuilder builder)
        => Configure(builder.MapMethods(Path, Verbs, InternalHandlerAsync));

    protected virtual RouteHandlerBuilder Configure(RouteHandlerBuilder builder)
        => builder;

    protected abstract Task<IResult> HandleAsync(CancellationToken cancellationToken);

    private Task<IResult> InternalHandlerAsync(CancellationToken cancellationToken)
        => HandleAsync(cancellationToken);
}
public abstract record Endpoint<TRequest>(string[] Verbs, string Path) : IEndpoint
{
    public Endpoint(string verb, string path) : this(new[] { verb }, path) { }

    public RouteHandlerBuilder Configure(IEndpointRouteBuilder builder)
        => Configure(builder.MapMethods(Path, Verbs, InternalHandler));

    protected virtual RouteHandlerBuilder Configure(RouteHandlerBuilder builder)
        => builder;

    protected abstract IResult Handle(TRequest request);

    private IResult InternalHandler([AsParameters]TRequest request)
        => Handle(request);
}
public abstract record AsyncEndpoint<TRequest>(string[] Verbs, string Path) : IEndpoint
{
    public AsyncEndpoint(string verb, string path) : this(new[] { verb }, path) { }

    public RouteHandlerBuilder Configure(IEndpointRouteBuilder builder)
        => Configure(builder.MapMethods(Path, Verbs, InternalHandlerAsync));

    protected virtual RouteHandlerBuilder Configure(RouteHandlerBuilder builder)
        => builder;

    protected abstract Task<IResult> HandleAsync(TRequest request, CancellationToken cancellationToken);

    private Task<IResult> InternalHandlerAsync([AsParameters]TRequest request, CancellationToken cancellationToken)
        => HandleAsync(request, cancellationToken);
}
public abstract record Delete(string Path) : Endpoint("DELETE", Path);
public abstract record Get(string Path) : Endpoint("GET", Path);
public abstract record Patch(string Path) : Endpoint("PATCH", Path);
public abstract record Post(string Path) : Endpoint("POST", Path);
public abstract record Put(string Path) : Endpoint("PUT", Path);
public abstract record Delete<TRequest>(string Path) : Endpoint<TRequest>("DELETE", Path);
public abstract record Get<TRequest>(string Path) : Endpoint<TRequest>("GET", Path);
public abstract record Patch<TRequest>(string Path) : Endpoint<TRequest>("PATCH", Path);
public abstract record Post<TRequest>(string Path) : Endpoint<TRequest>("POST", Path);
public abstract record Put<TRequest>(string Path) : Endpoint<TRequest>("PUT", Path);
public abstract record DeleteAsync(string Path) : AsyncEndpoint("DELETE", Path);
public abstract record GetAsync(string Path) : AsyncEndpoint("GET", Path);
public abstract record PatchAsync(string Path) : AsyncEndpoint("PATCH", Path);
public abstract record PostAsync(string Path) : AsyncEndpoint("POST", Path);
public abstract record PutAsync(string Path) : AsyncEndpoint("PUT", Path);
public abstract record DeleteAsync<TRequest>(string Path) : AsyncEndpoint<TRequest>("DELETE", Path);
public abstract record GetAsync<TRequest>(string Path) : AsyncEndpoint<TRequest>("GET", Path);
public abstract record PatchAsync<TRequest>(string Path) : AsyncEndpoint<TRequest>("PATCH", Path);
public abstract record PostAsync<TRequest>(string Path) : AsyncEndpoint<TRequest>("POST", Path);
public abstract record PutAsync<TRequest>(string Path) : AsyncEndpoint<TRequest>("PUT", Path);
public static class EndpointsExtensions
{
    public static RouteGroupBuilder MapEndpoints(this IEndpointRouteBuilder endpoints, Assembly assembly = null)
    {
        var logger = endpoints.ServiceProvider.GetRequiredService<ILogger<IEndpoint>>();
        assembly ??= Assembly.GetCallingAssembly();

        var group = endpoints.MapGroup("/");
        var endpointTypes = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && typeof(IEndpoint).IsAssignableFrom(t));
        foreach (var type in endpointTypes)
        {
            if (!type.GetConstructors().Any(c => c.GetParameters().Length == 0))
            {
                continue;
            }

            var endpoint = Activator.CreateInstance(type) as IEndpoint;
            if (endpoint is null)
            {
                continue;
            }

            logger.LogDebug("Configuring endpoint {Endpoint}", type.FullName);
            endpoint.Configure(group);
        }

        return group;
    }
}