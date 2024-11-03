# DaGangstaCoder.Rico.ValueObjects

## Example usage

```csharp
public sealed record MovieTitle() : ValueObject<string>(Length.Max(50), Unicode.None, Precision.None);
```

```csharp
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        foreach (var type in GetType().Assembly.GetTypes())
        {
            configurationBuilder.ApplyValueObjectConvention(type);
        }
    }
}
```
