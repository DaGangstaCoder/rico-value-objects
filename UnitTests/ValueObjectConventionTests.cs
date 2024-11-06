using Rico.ValueObjects;

namespace UnitTests;

public sealed class ValueObjectConventionTests
{
    [Fact]
    public void ValidateConventionOptions_ShouldDoNothing_WhenConventionOptionsIsNull()
    {
        var ctor = typeof(SealedTypeWithoutPrivateConstructor).GetConstructors().Single();

        ValueObjectConvention.ValidateConventionOptions(typeof(SealedTypeWithoutPrivateConstructor), ctor, null);
    }

    [Fact]
    public void ValidateConventionOptions_ShouldThrowException_WhenSealedTypeIsRequiredButTypeIsNotSealed()
    {
        var options = new ValueObjectConventionOptions.Builder()
            .RequireSealedType()
            .Options;

        Assert.Throws<InvalidOperationException>(() => ValueObjectConvention.ValidateConventionOptions(
            typeof(NotSealedType),
            typeof(NotSealedType).GetConstructors().Single(),
            options));
    }

    [Fact]
    public void ValidateConventionOptions_ShouldThrowException_WhenPrivateConstructorIsRequiredButNoneExists()
    {
        var options = new ValueObjectConventionOptions.Builder()
            .RequirePrivateConstructor()
            .Options;

        Assert.Throws<InvalidOperationException>(() => ValueObjectConvention.ValidateConventionOptions(
            typeof(SealedTypeWithoutPrivateConstructor),
            typeof(SealedTypeWithoutPrivateConstructor).GetConstructors().Single(),
            options));
    }
    
    [Fact]
    public void ApplyValueObjectConvention_ShouldConfigurePropertyConventionsCorrectly()
    {
        var wrapper = new PropertiesConfigurationBuilderWrapper(null);
        
        ValueObjectConvention.ApplyValueObjectConvention(wrapper, typeof(ParametrizedType));
        
        Assert.Equal(ParametrizedType.MaxLength, wrapper.Length);
        Assert.Equal(ParametrizedType.UnicodeAllowed, wrapper.Unicode);
        Assert.Equal(ParametrizedType.PrecisionValue, wrapper.Precision);
    }
    
    private record NotSealedType() : ValueObject<string>(Length.None, Unicode.None, Precision.None);
    
    public sealed record SealedTypeWithoutPrivateConstructor() : ValueObject<string>(
        Length.None,
        Unicode.None,
        Precision.None);

    private sealed record ParametrizedType() : ValueObject<string>(MaxLength, UnicodeAllowed, PrecisionValue)
    {
        public static readonly Length MaxLength = Length.Max(150);
        public static readonly Unicode UnicodeAllowed = Unicode.Allowed;
        public static readonly Precision PrecisionValue = Precision.Of(18, 6);
    }
}
