using Autofac.Builder;

namespace MediatR.Extensions.Autofac.DependencyInjection.Extensions;

internal static class RegistrationBuilderExtensions
{
    public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> ApplyTargetScope<TLimit, TActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder,
        RegistrationScope registrationScope)
    {
        switch (registrationScope)
        {
            case RegistrationScope.Transient:
                builder.InstancePerDependency();
                break;
            case RegistrationScope.Scoped:
                builder.InstancePerLifetimeScope();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(registrationScope), registrationScope, $"{registrationScope} is not supported!");
        }

        return builder;
    }
}