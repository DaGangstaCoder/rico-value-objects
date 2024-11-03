using Rico.ValueObjects;

namespace Sandbox.Movies;

public sealed record MovieReleaseDate() : ValueObject<DateOnly>(Length.None, Unicode.None, Precision.None);
