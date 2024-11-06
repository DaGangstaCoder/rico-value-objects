using Rico.ValueObjects;

namespace UnitTests;

public sealed class GenericTypeCheckerTests
{
    [Fact]
    public void IsAssignableToGenericType_ShouldReturnTrue_WhenGenericTypeMatches()
    {
        Assert.True(
            GenericTypeChecker.IsAssignableToGenericType(typeof(StringValueObject), typeof(ValueObject<string>)));
    }
    
    [Fact]
    public void IsAssignableToGenericType_ShouldReturnFalse_WhenGenericTypeDoesNotMatch()
    {
        Assert.False(
            GenericTypeChecker.IsAssignableToGenericType(typeof(StringValueObject), typeof(ValueObject<int>)));
    }

    private sealed record StringValueObject() : ValueObject<string>(Length.None, Unicode.None, Precision.None);
}
