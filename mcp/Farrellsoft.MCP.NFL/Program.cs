using Farrellsoft.MCP.NFL.Clients;
using Farrellsoft.MCP.NFL.Configuration;
using Farrellsoft.MCP.NFL.Mcp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ModelContextProtocol.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration setup
builder.Services.Configure<NflOptions>(builder.Configuration.GetSection("NFL"));

// HTTP client for external NFL API (placeholder)
builder.Services.AddHttpClient<INflApiClient, NflApiClient>((serviceProvider, httpClient) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<NflOptions>>().Value;
    var baseUrl = string.IsNullOrWhiteSpace(options.BaseUrl)
        ? "https://api.example.com/"
        : options.BaseUrl!.TrimEnd('/') + "/";
    httpClient.BaseAddress = new Uri(baseUrl);
});

// MCP server implementation
builder.Services.AddScoped<INflMcpServer, NflMcpServer>();

// Add MCP server over HTTP and register tools from this assembly
builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

// Expose the MCP HTTP endpoint at /mcp
app.MapMcp("/mcp");

app.Run();
