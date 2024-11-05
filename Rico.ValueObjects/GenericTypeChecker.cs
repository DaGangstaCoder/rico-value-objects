namespace Rico.ValueObjects;

internal static class GenericTypeChecker
{
    internal static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        if (genericType.IsAssignableFrom(givenType))
        {
            return true;
        }

        if (!genericType.IsGenericTypeDefinition)
        {
            return false;
        }

        return givenType.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType) ||
            IsAssignableToGenericTypeRecursive(givenType.BaseType, genericType);
    }

    private static bool IsAssignableToGenericTypeRecursive(Type? baseType, Type genericType)
    {
        while (baseType is not null)
        {
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            baseType = baseType.BaseType;
        }

        return false;
    }
}
