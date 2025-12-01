namespace nfl_api.Models;

public class NflGameResponse
{
    public string Title { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public short Season { get; set; }
    public short WeekNo { get; set; }
    public DateOnly Date { get; set; }
}
