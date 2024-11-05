namespace Rico.ValueObjects;

public sealed class ValueObjectConventionOptions
{
    internal ValueObjectConventionOptions() { }

    internal bool IsPrivateConstructorRequired { get; private set; }

    internal bool IsSealedTypeRequired { get; private set; }

    public sealed class Builder
    {
        public ValueObjectConventionOptions Options { get; } = new();

        public Builder RequirePrivateConstructor(bool required = true)
        {
            Options.IsPrivateConstructorRequired = required;
            return this;
        }

        public Builder RequireSealedType(bool required = true)
        {
            Options.IsSealedTypeRequired = required;
            return this;
        }
    }
}
