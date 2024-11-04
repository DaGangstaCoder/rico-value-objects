using Microsoft.EntityFrameworkCore;
using Rico.ValueObjects;
using Sandbox.Movies;

namespace Sandbox;

public sealed class SandboxDbContext(DbContextOptions<SandboxDbContext> options) : DbContext(options)
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
