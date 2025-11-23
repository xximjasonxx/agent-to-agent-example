using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Farrellsoft.MCP.Weather.Clients;

/// <summary>
/// Client for interacting with the api.weather.com service.
/// Ensures that the configured API key is always sent as the <c>apiKey</c> query parameter.
/// </summary>
public interface IWeatherApiClient
{
/// <summary>
/// Retrieves the current weather observation for the specified postal code
/// using the <c>v1/current.json</c> endpoint on api.weatherapi.com.
/// </summary>
/// <param name="postalCode">
/// The postal code value that will be passed to api.weatherapi.com as the <c>q</c> query parameter.
/// </param>
/// <param name="cancellationToken">Cancellation token.</param>
/// <returns>The JSON payload returned by api.weatherapi.com.</returns>
Task<JsonElement> GetCurrentWeatherByPostalCodeAsync(string postalCode, CancellationToken cancellationToken = default);
}
