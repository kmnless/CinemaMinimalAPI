namespace CinemaMinimalAPI.DTO;

public class SessionDTO
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int CinemaId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
