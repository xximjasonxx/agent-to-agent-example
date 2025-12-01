using System.ComponentModel;
using ModelContextProtocol.Server;

namespace Farrellsoft.MCP.NFL.Mcp;

[McpServerToolType]
public static class NflTools
{
    [McpServerTool]
    [Description("Get games played for a given NFL season and week")]
    public static async Task<string> GetGamesPlayed(
        [Description("The NFL season year (e.g., 2024)")] int season,
        [Description("The week number (1-18 for regular season)")] int week,
        INflMcpServer server)
    {
        return await server.GetGamesPlayedAsync(season, week);
    }
}
