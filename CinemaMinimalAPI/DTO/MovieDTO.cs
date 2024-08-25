namespace CinemaMinimalAPI.DTO;

public class MovieDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int DurationInMinutes { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
}

