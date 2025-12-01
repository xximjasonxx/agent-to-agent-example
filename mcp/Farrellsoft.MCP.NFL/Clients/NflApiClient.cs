namespace Farrellsoft.MCP.NFL.Clients;

public class NflApiClient : INflApiClient
{
    private readonly HttpClient _httpClient;

    public NflApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<string> GetScheduleAsync(int season, int week, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"schedule/{season}/week/{week}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
