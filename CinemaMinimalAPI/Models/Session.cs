namespace CinemaMinimalAPI.Models;

public class Session
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MovieId { get; set; }
    public virtual Movie Movie { get; set; }
    public int CinemaId { get; set; }
    public virtual Cinema Cinema { get; set; }
}
