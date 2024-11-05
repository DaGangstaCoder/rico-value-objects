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

    public record NotSealedType() : ValueObject<string>(Length.None, Unicode.None, Precision.None);

    public sealed record SealedTypeWithoutPrivateConstructor() : ValueObject<string>(Length.None, Unicode.None, Precision.None);
}
