using Farrellsoft.MCP.NFL.Clients;

namespace Farrellsoft.MCP.NFL.Mcp;

public class NflMcpServer : INflMcpServer
{
    private readonly INflApiClient _apiClient;

    public NflMcpServer(INflApiClient apiClient)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    // MCP server methods will be implemented here
}
