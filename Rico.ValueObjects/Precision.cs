namespace Rico.ValueObjects;

public sealed record Precision
{
    public static readonly Precision None = new(0, 0);

    private Precision(int precision, int scale)
    {
        PrecisionValue = precision;
        Scale = scale;
    }

    public int PrecisionValue { get; }

    public int Scale { get; }

    public static Precision Of(int precision, int scale = 0) => new(precision, scale);
}
