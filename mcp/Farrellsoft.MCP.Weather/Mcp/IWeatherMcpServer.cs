using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Farrellsoft.MCP.Weather.Mcp.Contracts;

namespace Farrellsoft.MCP.Weather.Mcp;

/// <summary>
/// MCP server interface that exposes weather-related tools.
/// </summary>
public interface IWeatherMcpServer
{
    /// <summary>
    /// Gets the current weather observation for the specified postal code.
    /// This corresponds to the MCP tool <c>GetWeatherByPostalCode</c> and
    /// internally calls api.weatherapi.com at
    /// <c>v1/current.json?key=&lt;key&gt;&amp;q=&lt;postalCode&gt;&amp;aqi=no</c>.
    /// </summary>
    /// <param name="request">Request containing the postal code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A simplified response DTO containing location and current conditions.</returns>
    Task<GetWeatherForPostalCodeResponse> GetWeatherByPostalCodeAsync(GetWeatherByPostalCodeRequest request, CancellationToken cancellationToken = default);
}
