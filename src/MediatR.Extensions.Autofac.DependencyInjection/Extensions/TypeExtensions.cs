using System.Reflection;

namespace MediatR.Extensions.Autofac.DependencyInjection.Extensions;

internal static class TypeExtensions
{
    public static ConstructorInfo GetFirstPublicConstructor(this Type type)
        => type.GetConstructors( BindingFlags.Instance | BindingFlags.Public)[0];
}