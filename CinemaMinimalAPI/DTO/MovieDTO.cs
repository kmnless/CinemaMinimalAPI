using CinemaMinimalAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace CinemaMinimalAPI.DTO;

public class MovieDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes.")]
    public int DurationInMinutes { get; set; }
    public Genre? Genre { get; set; }
    public Director? Director { get; set; }
}

