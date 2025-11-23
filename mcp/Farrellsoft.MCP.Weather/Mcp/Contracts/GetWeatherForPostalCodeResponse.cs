namespace Farrellsoft.MCP.Weather.Mcp.Contracts;

/// <summary>
/// Simplified response returned by the <c>GetWeatherByPostalCode</c> MCP tool and HTTP endpoint.
/// </summary>
public sealed class GetWeatherForPostalCodeResponse
{
    /// <summary>
    /// Name of the nearest location, mapped from <c>location.name</c>.
    /// </summary>
    public string LocationName { get; set; } = string.Empty;

    /// <summary>
    /// Region of the nearest location, mapped from <c>location.region</c>.
    /// </summary>
    public string LocationRegion { get; set; } = string.Empty;

    /// <summary>
    /// Postal code used for the query.
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// Temperature in degrees Fahrenheit, mapped from <c>current.temp_f</c>.
    /// </summary>
    public double Temp { get; set; }

    /// <summary>
    /// Wind speed in miles per hour, mapped from <c>current.wind_mph</c>.
    /// </summary>
    public double Wind { get; set; }

    /// <summary>
    /// Textual description of the current condition, mapped from <c>current.condition.text</c>.
    /// </summary>
    public string ConditionDescription { get; set; } = string.Empty;
}
