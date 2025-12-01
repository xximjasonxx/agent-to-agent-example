namespace Farrellsoft.MCP.NFL.Mcp;

public interface INflMcpServer
{
    Task<string> GetGamesPlayedAsync(int season, int week);
}
