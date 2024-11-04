using Rico.ValueObjects;

namespace Sandbox.Movies;

public sealed record MoviePrice : ValueObject<decimal>
{
    private MoviePrice() : base(Length.None, Unicode.None, Precision.Of(14, 2)) { }
    
    public static MoviePrice Of(decimal value) => new() { Value = value };
}
