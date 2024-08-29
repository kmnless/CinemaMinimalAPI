using System.ComponentModel.DataAnnotations;

namespace CinemaMinimalAPI.DTO;

public class AddMovieDTO
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes.")]
    public int DurationInMinutes { get; set; }

    [Required(ErrorMessage = "GenreId is required.")]
    public int GenreId { get; set; }

    [Required(ErrorMessage = "DirectorId is required.")]
    public int DirectorId { get; set; }
}
