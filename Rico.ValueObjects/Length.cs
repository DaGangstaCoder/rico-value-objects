namespace Rico.ValueObjects;

public sealed record Length
{
    public static readonly Length None = new(0, false);

    private Length(int value, bool exact)
    {
        Value = value;
        IsExact = exact;
    }

    public int Value { get; }

    public bool IsExact { get; }

    public static Length Exact(int length) => new(length, true);

    public static Length Max(int length) => new(length, false);
}
