using System.ComponentModel;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Farrellsoft.MCP.Weather.Mcp.Contracts;
using ModelContextProtocol.Server;

namespace Farrellsoft.MCP.Weather.Mcp;

/// <summary>
/// MCP tool definitions for weather-related operations.
/// </summary>
[McpServerToolType]
public static class WeatherTools
{
    /// <summary>
    /// Gets the current weather observation for the specified postal code.
    /// This MCP tool delegates to <see cref="IWeatherMcpServer"/> which in turn
    /// calls api.weatherapi.com at
    /// <c>v1/current.json?key=&lt;key&gt;&amp;q=&lt;postalCode&gt;&amp;aqi=no</c>.
    /// </summary>
    /// <param name="server">The weather MCP server implementation to delegate to.</param>
    /// <param name="postalCode">
    /// Postal code value that will be passed to api.weatherapi.com as the
    /// <c>q</c> query parameter.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A simplified response DTO containing location and current conditions.</returns>
    [McpServerTool(Name = "GetWeatherByPostalCode"), Description("Retrieves the current weather observation for a given postal code using api.weatherapi.com.")]
    public static Task<GetWeatherForPostalCodeResponse> GetWeatherByPostalCodeAsync(
        IWeatherMcpServer server,
        [Description("Postal code value to search by")] string postalCode,
        CancellationToken cancellationToken = default)
    {
        var request = new Contracts.GetWeatherByPostalCodeRequest
        {
            PostalCode = postalCode,
        };

        return server.GetWeatherByPostalCodeAsync(request, cancellationToken);
    }
}
