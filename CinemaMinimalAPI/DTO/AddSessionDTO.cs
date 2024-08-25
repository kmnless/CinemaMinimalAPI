namespace CinemaMinimalAPI.DTO;

public class AddSessionDTO
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MovieId { get; set; } 
}
