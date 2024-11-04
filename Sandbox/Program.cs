using Microsoft.EntityFrameworkCore;
using Sandbox;
using Sandbox.Movies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SandboxDbContext>(options =>
    options.UseSqlServer(
        "Server=localhost,1434; Database=mozart; User Id=sa; Password=Qwerty1@; TrustServerCertificate=True;"));

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await using var db = scope.ServiceProvider.GetRequiredService<SandboxDbContext>();

db.Database.EnsureDeleted();
db.Database.EnsureCreated();

db.Movies.Add(new Movie
{
    Price = MoviePrice.Of(9.99m),
    Title = MovieTitle.Create("The Dark Knight"),
    ReleaseDate = MovieReleaseDate.Create(new DateOnly(2008, 09, 12)),
});

await db.SaveChangesAsync();   

app.Run();
