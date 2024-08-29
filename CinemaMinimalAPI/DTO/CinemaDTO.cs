using System.ComponentModel.DataAnnotations;

namespace CinemaMinimalAPI.DTO;

public class CinemaDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Location is required")]
    [StringLength(200, ErrorMessage = "Location can't be longer than 200 characters")]
    public string Location { get; set; }
}