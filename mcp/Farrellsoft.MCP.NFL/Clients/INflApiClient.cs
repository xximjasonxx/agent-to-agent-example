namespace Farrellsoft.MCP.NFL.Clients;

public interface INflApiClient
{
    Task<string> GetScheduleAsync(int season, int week, CancellationToken cancellationToken = default);
}
