# DaGangstaCoder.Rico.ValueObjects

<img src="logo-thumbnail.png" alt="DaGangstaCoder.Rico.ValueObjects Logo" width="512" />

`DaGangstaCoder.Rico.ValueObjects` provides a structured approach to handling value objects in C# applications, making it easy to encapsulate specific business rules and constraints within your domain models.

## Getting Started with Value Objects

### Step 1: Define an Entity Class

Create your entity class and use specific value object types for its properties to ensure domain-specific constraints.

```csharp
public class Movie
{
    public int Id { get; private set; }
    
    public required MovieTitle Title { get; init; }
    
    public required MoviePrice Price { get; init; }
    
    public required MovieReleaseDate ReleaseDate { get; init; }
}
```

### Step 2: Define Value Object Types

Define custom value object types for each property, encapsulating business rules directly within each class.

```csharp
public sealed record MovieTitle : ValueObject<string>
{
    private MovieTitle() : base(Length.Max(50), Unicode.Allowed, Precision.None) { }
    
    public static MovieTitle Create(string value) => new() { Value = value };
}
```

```csharp
public sealed record MoviePrice : ValueObject<decimal>
{
    private MoviePrice() : base(Length.None, Unicode.None, Precision.Of(14, 2)) { }
    
    public static MoviePrice Of(decimal value) => new() { Value = value };
}
```

```csharp
public sealed record MovieReleaseDate : ValueObject<DateOnly>
{
    private MovieReleaseDate() : base(Length.None, Unicode.None, Precision.None) { }
    
    public static MovieReleaseDate Create(DateOnly value) => new() { Value = value };
}
```

### Step 3: Apply Conventions for Value Objects in DbContext

In your `DbContext` class, apply conventions for value objects automatically, ensuring consistency across your models.

```csharp
public class AppDbContext : DbContext
{
    public DbSet<Movie> Movies => Set<Movie>();
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        foreach (var type in GetType().Assembly.GetTypes())
        {
            configurationBuilder.ApplyValueObjectConvention(type, o => o.RequirePrivateConstructor());
        }
    }
}
```

### Example Usage

```csharp
using var context = new AppDbContext();

context.Movies.Add(new Movie
{
    Price = MoviePrice.Of(9.99m),
    Title = MovieTitle.Create("The Dark Knight"),
    ReleaseDate = MovieReleaseDate.Create(new DateOnly(2008, 07, 25)),
});

context.SaveChanges();
```

With these steps, you can create well-structured, constraint-enforced value objects in your applications, enhancing domain-driven design practices.
