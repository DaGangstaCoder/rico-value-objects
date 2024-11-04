using Rico.ValueObjects;

namespace Sandbox.Movies;

public sealed record MovieReleaseDate : ValueObject<DateOnly>
{
    private MovieReleaseDate() : base(Length.None, Unicode.None, Precision.None) { }
    
    public static MovieReleaseDate Create(DateOnly value) => new() { Value = value };
}
