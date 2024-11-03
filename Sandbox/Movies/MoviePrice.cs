using Rico.ValueObjects;

namespace Sandbox.Movies;

public sealed record MoviePrice() : ValueObject<decimal>(Length.None, Unicode.None, Precision.Of(14, 2));
