using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Rico.ValueObjects;

public sealed class ValueObjectValueConverter<TModel, TValue>() : ValueConverter<TModel, TValue>(
    model => model.Value,
    value => CreateInstance(value))
    where TModel : ValueObject<TValue>
    where TValue : IComparable<TValue>
{
    private static TModel CreateInstance(TValue value)
    {
        var model = Activator.CreateInstance(typeof(TModel), nonPublic: true) as TModel ??
                    throw new ArgumentNullException(nameof(value));
        
        typeof(TModel).GetProperty("Value")!.SetValue(model, value);
        return model;
    }
}
