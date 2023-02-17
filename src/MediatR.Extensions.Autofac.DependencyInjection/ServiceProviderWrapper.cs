using Autofac;

namespace MediatR.Extensions.Autofac.DependencyInjection;

internal class ServiceProviderWrapper : IServiceProvider
{
    private readonly ILifetimeScope _lifeTimeScope;

    public ServiceProviderWrapper(ILifetimeScope lifeTimeScope)
    {
        this._lifeTimeScope = lifeTimeScope;
    }

    public object GetService(Type serviceType) => this._lifeTimeScope.ResolveOptional(serviceType);
}