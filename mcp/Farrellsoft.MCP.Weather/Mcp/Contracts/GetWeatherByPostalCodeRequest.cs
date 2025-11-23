namespace Farrellsoft.MCP.Weather.Mcp.Contracts;

/// <summary>
/// Request payload for the <c>GetWeatherByPostalCode</c> MCP endpoint.
/// </summary>
public sealed class GetWeatherByPostalCodeRequest
{
    /// <summary>
    /// Postal code value that will be passed to api.weatherapi.com as the <c>q</c> query parameter.
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;
}
