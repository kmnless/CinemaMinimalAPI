using CinemaMinimalAPI.Models;

namespace CinemaMinimalAPI.DTO;

public class MovieDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationInMinutes { get; set; }
    public Genre? Genre { get; set; }
    public Director? Director { get; set; }
}

