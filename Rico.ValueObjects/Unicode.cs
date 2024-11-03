namespace Rico.ValueObjects;

public sealed record Unicode
{
    public static readonly Unicode None = new(false);

    public static readonly Unicode Allowed = new(true);

    private Unicode(bool allowed)
    {
        IsAllowed = allowed;
    }

    public bool IsAllowed { get; }
}
