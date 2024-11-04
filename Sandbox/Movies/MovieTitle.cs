using Rico.ValueObjects;

namespace Sandbox.Movies;

public sealed record MovieTitle : ValueObject<string>
{
    private MovieTitle() : base(Length.Max(50), Unicode.Allowed, Precision.None) { }
    
    public static MovieTitle Create(string value) => new() { Value = value };
}
