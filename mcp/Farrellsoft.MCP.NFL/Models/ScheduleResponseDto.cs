namespace Farrellsoft.MCP.NFL.Models;

public class ScheduleResponseDto
{
    public int Season { get; set; }
    public int Week { get; set; }
    public List<GameDto> Games { get; set; } = new();
}
