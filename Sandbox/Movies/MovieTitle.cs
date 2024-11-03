using Rico.ValueObjects;

namespace Sandbox.Movies;

public sealed record MovieTitle() : ValueObject<string>(Length.Max(50), Unicode.None, Precision.None);
