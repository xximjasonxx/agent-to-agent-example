using System;
using System.Text.Json;
using Farrellsoft.MCP.Weather.Mcp.Contracts;
using System.Threading;
using System.Threading.Tasks;
using Farrellsoft.MCP.Weather.Clients;

namespace Farrellsoft.MCP.Weather.Mcp;

/// <summary>
/// MCP server implementation that exposes weather-related tools.
/// </summary>
public sealed class WeatherMcpServer : IWeatherMcpServer
{
    private readonly IWeatherApiClient _weatherApiClient;

    public WeatherMcpServer(IWeatherApiClient weatherApiClient)
    {
        _weatherApiClient = weatherApiClient;
    }

    /// <inheritdoc />
    public async Task<GetWeatherForPostalCodeResponse> GetWeatherByPostalCodeAsync(GetWeatherByPostalCodeRequest request, CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.PostalCode))
        {
            throw new ArgumentException("Postal code is required.", nameof(request.PostalCode));
        }

        var json = await _weatherApiClient.GetCurrentWeatherByPostalCodeAsync(request.PostalCode, cancellationToken).ConfigureAwait(false);

        // Map the provider JSON into the simplified response DTO.
        var location = json.GetProperty("location");
        var current = json.GetProperty("current");
        var condition = current.GetProperty("condition");

        return new GetWeatherForPostalCodeResponse
        {
            LocationName = location.GetProperty("name").GetString() ?? string.Empty,
            LocationRegion = location.GetProperty("region").GetString() ?? string.Empty,
            PostalCode = request.PostalCode,
            Temp = current.GetProperty("temp_f").GetDouble(),
            Wind = current.GetProperty("wind_mph").GetDouble(),
            ConditionDescription = condition.GetProperty("text").GetString() ?? string.Empty,
        };
    }
}
