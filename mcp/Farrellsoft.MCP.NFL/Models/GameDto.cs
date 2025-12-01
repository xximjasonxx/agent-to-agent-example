namespace Farrellsoft.MCP.NFL.Models;

public class GameDto
{
    public string Title { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public int Season { get; set; }
    public int WeekNo { get; set; }
    public string Date { get; set; } = string.Empty;
}
