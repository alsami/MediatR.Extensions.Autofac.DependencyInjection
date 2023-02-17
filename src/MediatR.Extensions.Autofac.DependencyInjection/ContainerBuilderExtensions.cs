using Autofac;

namespace MediatR.Extensions.Autofac.DependencyInjection;

public static class ContainerBuilderExtensions
{
    public static ContainerBuilder RegisterMediatR(
        this ContainerBuilder builder,
        MediatRConfiguration configuration)
    {
        builder.RegisterModule(new MediatRModule(configuration));

        return builder;
    }
}