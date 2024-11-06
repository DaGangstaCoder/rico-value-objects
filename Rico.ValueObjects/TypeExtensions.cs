using System.Reflection;

namespace Rico.ValueObjects;

internal static class TypeExtensions
{
    private const BindingFlags PropertyBindingFlags =
        BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

    private const BindingFlags ConstructorBindingFlags =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    internal static T GetPropertyValueOrThrow<T>(this Type type, string propertyName, object valueObject)
        where T : class
    {
        var property = type.GetProperty(propertyName, PropertyBindingFlags) ??
            throw new ArgumentException($"Property '{propertyName}' not found in type '{type.FullName}'.");

        return property.GetValue(valueObject) as T ??
            throw new ArgumentException($"Property '{propertyName}' value in type '{type.FullName}' cannot be null.");
    }

    internal static ConstructorInfo GetParameterlessConstructor(this Type type)
    {
        var constructors = type.GetConstructors(ConstructorBindingFlags);

        var parameterlessConstructor = constructors.SingleOrDefault(c => c.GetParameters().Length == 0) ??
            throw new InvalidOperationException(
                $"A parameterless constructor is required for value object of type '{type.FullName}'.");

        return parameterlessConstructor;
    }
}
