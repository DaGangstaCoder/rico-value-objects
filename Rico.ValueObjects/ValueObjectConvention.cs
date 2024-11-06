using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Rico.ValueObjects;

public static class ValueObjectConvention
{
    public static void ApplyValueObjectConvention(this ModelConfigurationBuilder configurationBuilder,
        Type type,
        ValueObjectConventionOptions? options = null)
    {
        if (!GenericTypeChecker.IsAssignableToGenericType(type, typeof(ValueObject<>)))
        {
            return;
        }

        var propertyConventionBuilder = CreatePropertyBuilder(type, configurationBuilder);

        ApplyValueObjectConvention(propertyConventionBuilder, type, options);
    }

    internal static void ApplyValueObjectConvention(
        PropertiesConfigurationBuilderWrapper propertyConventionBuilder,
        Type type,
        ValueObjectConventionOptions? options = null)
    {
        var parameterlessConstructor = type.GetParameterlessConstructor();

        ValidateConventionOptions(type, parameterlessConstructor, options);

        var valueObject = parameterlessConstructor.Invoke([]);

        ConfigureLength(type, valueObject, propertyConventionBuilder);

        ConfigureUnicode(type, valueObject, propertyConventionBuilder);

        ConfigurePrecision(type, valueObject, propertyConventionBuilder);
    }

    internal static void ValidateConventionOptions(Type type, ConstructorInfo constructor,
        ValueObjectConventionOptions? options)
    {
        if (options is null)
        {
            return;
        }

        if (options.IsSealedTypeRequired && !type.IsSealed)
        {
            throw new InvalidOperationException(
                $"The value object of type '{type.FullName}' is required to be sealed.");
        }

        if (options.IsPrivateConstructorRequired && !constructor.IsPrivate)
        {
            throw new InvalidOperationException(
                $"A private parameterless constructor is required for value object of type '{type.FullName}'.");
        }
    }

    private static PropertiesConfigurationBuilderWrapper CreatePropertyBuilder(Type type,
        ModelConfigurationBuilder configurationBuilder)
    {
        var genericArguments = type.BaseType!.GetGenericArguments();
        var genericTypeArgument = genericArguments[0];

        var converterType = typeof(ValueObjectValueConverter<,>).MakeGenericType([type, genericTypeArgument]);

        var propertyBuilder = configurationBuilder.Properties(type);
        propertyBuilder.HaveConversion(converterType);

        return new PropertiesConfigurationBuilderWrapper(propertyBuilder);
    }

    private static void ConfigureLength(Type type, object valueObject,
        PropertiesConfigurationBuilderWrapper conventionBuilder)
    {
        const string lengthPropertyName = nameof(ValueObject<int>.Length);

        var length = type.GetPropertyValueOrThrow<Length>(lengthPropertyName, valueObject);

        conventionBuilder.SetLength(length);
    }

    private static void ConfigureUnicode(Type type, object valueObject,
        PropertiesConfigurationBuilderWrapper propertyBuilder)
    {
        const string unicodePropertyName = nameof(ValueObject<int>.Unicode);

        var unicode = type.GetPropertyValueOrThrow<Unicode>(unicodePropertyName, valueObject);

        propertyBuilder.SetUnicode(unicode);
    }

    private static void ConfigurePrecision(Type type, object valueObject,
        PropertiesConfigurationBuilderWrapper propertyBuilder)
    {
        const string precisionPropertyName = nameof(ValueObject<int>.Precision);

        var precision = type.GetPropertyValueOrThrow<Precision>(precisionPropertyName, valueObject);

        propertyBuilder.SetPrecision(precision);
    }
}
