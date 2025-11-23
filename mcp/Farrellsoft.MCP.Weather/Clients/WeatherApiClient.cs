using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Farrellsoft.MCP.Weather.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Farrellsoft.MCP.Weather.Clients;

/// <inheritdoc />
public sealed class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly WeatherOptions _options;

    public WeatherApiClient(HttpClient httpClient, IOptions<WeatherOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    /// <inheritdoc />
    public async Task<JsonElement> GetCurrentWeatherByPostalCodeAsync(string postalCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
        {
            throw new ArgumentException("Postal code must be provided.", nameof(postalCode));
        }

        // API key now comes from the "ApiKey" environment variable.
        var apiKey = Environment.GetEnvironmentVariable("ApiKey");
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Weather API key environment variable 'ApiKey' is not configured.");
        }

        var queryString = new Dictionary<string, string?>
        {
            ["key"] = apiKey,
            ["q"] = postalCode,
            ["aqi"] = "no",
        };

        // This will build: v1/current.json?key=<key>&q=<postalCode>&aqi=no
        var requestUri = QueryHelpers.AddQueryString("v1/current.json", queryString);

        using var response = await _httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken).ConfigureAwait(false);

        return json;
    }
}
