namespace Farrellsoft.MCP.Weather.Configuration;

/// <summary>
/// Configuration options for accessing the external weather API.
/// </summary>
public sealed class WeatherOptions
{
    /// <summary>
    /// (Deprecated) API key previously used for api.weather.com.
    /// The current implementation reads the API key from the <c>ApiKey</c> environment variable.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// Base URL for the weather API. Defaults to <c>https://api.weatherapi.com/</c> when not specified.
    /// </summary>
    public string? BaseUrl { get; set; }
}
