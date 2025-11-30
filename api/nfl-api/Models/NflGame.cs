namespace nfl_api.Models;

public class NflGame
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public short Season { get; set; }
    public short WeekNo { get; set; }
    public DateOnly Date { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
