using System.IO;

namespace CinemaMinimalAPI.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationInMinutes { get; set; }
    public int GenreId { get; set; }
    public virtual Genre Genre { get; set; }
    public int DirectorId { get; set; }
    public virtual Director Director { get; set; }
}
