using System.Reflection;

namespace MediatR.Extensions.Autofac.DependencyInjection.Extensions;

internal static class TypeExtensions
{
    public static ConstructorInfo GetRequiredPublicConstructor(this Type type, params Type[] parameterTypes)
        => type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, parameterTypes, null)
           ?? throw new InvalidOperationException(
               $"Type '{type.FullName}' has no public constructor with parameters " +
               $"({string.Join(", ", parameterTypes.Select(parameterType => parameterType.FullName))}).");
}