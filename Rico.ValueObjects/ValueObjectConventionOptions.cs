namespace Rico.ValueObjects;

public sealed class ValueObjectConventionOptions
{
    internal bool IsPrivateConstructorRequired { get; private set; }
    
    internal bool IsSealedTypeRequired { get; private set; }
    
    internal ValueObjectConventionOptions() { }
    
    public ValueObjectConventionOptions RequirePrivateConstructor()
    {
        IsPrivateConstructorRequired = true;
        return this;
    }
    
    public ValueObjectConventionOptions RequireSealedType()
    {
        IsSealedTypeRequired = true;
        return this;
    }
}
