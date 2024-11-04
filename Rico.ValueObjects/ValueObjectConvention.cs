using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Rico.ValueObjects;

public static class ValueObjectConvention
{
    public static void ApplyValueObjectConvention(this ModelConfigurationBuilder configurationBuilder,
        Type type,
        Action<ValueObjectConventionOptions>? configureOptions = null)
    {
        if (!IsAssignableToGenericType(type, typeof(ValueObject<>)))
        {
            return;
        }

        var options = new ValueObjectConventionOptions();
        
        configureOptions?.Invoke(options);
        
        var baseType = type.BaseType;
        
        if (!baseType!.IsGenericType)
        {
            baseType = baseType.BaseType;
        }
        
        var genericArguments = baseType!.GetGenericArguments();
        var genericTypeArgument = genericArguments[0];

        var propertyBuilder = configurationBuilder.Properties(type);
        var converterType = typeof(ValueObjectValueConverter<,>).MakeGenericType([type, genericTypeArgument]);

        propertyBuilder.HaveConversion(converterType);

        var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var parameterlessConstructor = constructors.SingleOrDefault(c => c.GetParameters().Length == 0);
        if (parameterlessConstructor is null)
        {
            throw new InvalidOperationException(
                $"A parameterless constructor is required for value object of type {type.FullName}.");
        }
        
        if (options.IsPrivateConstructorRequired && !parameterlessConstructor.IsPrivate)
        {
            throw new InvalidOperationException(
                $"A private parameterless constructor is required for value object of type {type.FullName}.");
        }
        
        var valueObject = parameterlessConstructor.Invoke([]);
        
        var length = type
            .GetProperty("Length",
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.FlattenHierarchy)!
            .GetValue(valueObject)
            as Length;

        if (length!.Value != Length.None.Value)
        {
            propertyBuilder.HaveMaxLength(length.Value);
        }

        if (length.IsExact)
        {
            propertyBuilder.AreFixedLength();
        }

        var unicode = type
            .GetProperty("Unicode",
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.FlattenHierarchy)!
            .GetValue(valueObject)
            as Unicode;

        if (unicode!.IsAllowed)
        {
            propertyBuilder.AreUnicode();
        }

        var precision = type
            .GetProperty("Precision",
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.FlattenHierarchy)!
            .GetValue(valueObject)
            as Precision;

        if (precision != Precision.None)
        {
            propertyBuilder.HavePrecision(precision!.PrecisionValue, precision.Scale);
        }
    }
    
    private static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        if (genericType.IsAssignableFrom(givenType))
        {
            return true;
        }

        if (!genericType.IsGenericTypeDefinition)
        {
            return false;
            
        }

        if (givenType.GetInterfaces().Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
        {
            return true;
        }

        var baseType = givenType.BaseType;
        
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
