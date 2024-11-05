using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rico.ValueObjects;

internal sealed class PropertiesConfigurationBuilderWrapper(PropertiesConfigurationBuilder propertyBuilder)
{
    internal Type? ValueConverter { get; private set; }

    internal Length? Length { get; private set; }

    internal Unicode? Unicode { get; private set; }

    internal Precision? Precision { get; private set; }

    internal void SetConversion(Type conversionType)
    {
        ValueConverter = conversionType;
        propertyBuilder.HaveConversion(conversionType);
    }

    internal void SetLength(Length length)
    {
        Length = length;
        propertyBuilder.HaveMaxLength(length.Value);
        propertyBuilder.AreFixedLength(length.IsExact);
    }

    internal void SetUnicode(Unicode unicode)
    {
        Unicode = unicode;
        propertyBuilder.AreUnicode(unicode.IsAllowed);
    }

    internal void SetPrecision(Precision precision)
    {
        Precision = precision;
        propertyBuilder.HavePrecision(precision.PrecisionValue, precision.Scale);
    }
}
