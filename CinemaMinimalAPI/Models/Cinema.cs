namespace CinemaMinimalAPI.Models;

public class Cinema
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public virtual ICollection<Session> Sessions { get; set; }
}
