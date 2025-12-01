namespace Farrellsoft.MCP.NFL.Clients;

public class NflApiClient : INflApiClient
{
    private readonly HttpClient _httpClient;

    public NflApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    // API client methods will be implemented here
}
