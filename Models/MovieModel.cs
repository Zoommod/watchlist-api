namespace Watchlist.models;

public class MovieModel{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public string? Genre { get; set; }
    public int ReleaseYear { get; set; }
    public bool Watched { get; set; } = false;
    public int? Rating { get; set; }
    public string? Comment { get; set; }
}