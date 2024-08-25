namespace CinemaMinimalAPI.DTO;

public class AddMovieDTO
{
    public string Title { get; set; } = string.Empty;
    public int DurationInMinutes { get; set; }
    public int GenreId { get; set; }
    public int DirectorId { get; set; }
}
