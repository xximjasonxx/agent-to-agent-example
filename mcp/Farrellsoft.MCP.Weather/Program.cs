using Farrellsoft.MCP.Weather.Clients;
using Farrellsoft.MCP.Weather.Configuration;
using Farrellsoft.MCP.Weather.Mcp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ModelContextProtocol.AspNetCore; // important

var builder = WebApplication.CreateBuilder(args);

// existing WeatherOptions + HttpClient setup...

builder.Services.Configure<WeatherOptions>(builder.Configuration.GetSection("Weather"));

builder.Services.AddHttpClient<IWeatherApiClient, WeatherApiClient>((serviceProvider, httpClient) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<WeatherOptions>>().Value;
    var baseUrl = string.IsNullOrWhiteSpace(options.BaseUrl)
        ? "https://api.weatherapi.com/"
        : options.BaseUrl!.TrimEnd('/') + "/";
    httpClient.BaseAddress = new Uri(baseUrl);
});

// Your MCP server implementation
builder.Services.AddScoped<IWeatherMcpServer, WeatherMcpServer>();

// 👉 Add MCP server over HTTP and register tools from this assembly
builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly(); // discovers WeatherTools via [McpServerToolType]

var app = builder.Build();

// 👉 Expose the MCP HTTP endpoint at /mcp
app.MapMcp("/mcp");

app.Run();