namespace Sandbox.Movies;

public sealed class Movie
{
    public int Id { get; private set; }
    
    public required MovieTitle Title { get; init; }
    
    public required MoviePrice Price { get; init; }
    
    public required MovieReleaseDate ReleaseDate { get; init; }
}
