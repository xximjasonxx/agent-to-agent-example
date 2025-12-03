using A2A.AspNetCore;
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Extensions.AI;
using Backend.Models;
using Backend.Services;
using ModelContextProtocol.Client;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOpenApi();
//builder.Services.AddSwaggerGen();

// Register application services
string endpoint = builder.Configuration["AZURE_AI_ENDPOINT"]
    ?? throw new InvalidOperationException("AZURE_AI_ENDPOINT is not set.");
string deploymentName = "gpt-5-chat-deployment";

// Register the chat client with function invocation support
IChatClient chatClient = new ChatClientBuilder(
        new AzureOpenAIClient(
            new Uri(endpoint),
            new DefaultAzureCredential())
        .GetChatClient(deploymentName)
        .AsIChatClient())
    .Build();
builder.Services.AddSingleton(chatClient);

var weatherAgent = builder.AddAIAgent(
    name: "weather-agent",
    instructions: "You are a helpful weather agent that uses MCP tools to provide accurate and up-to-date weather information. Use the available tools to answer weather-related questions."
);

var nflAgent = builder.AddAIAgent(
    name: "nfl-agent",
    instructions: "You are a knowledgeable NFL agent that uses MCP tools to provide accurate and up-to-date information about NFL teams, players, and games. Use the available tools to answer NFL-related questions."
);

// Register the chat service
builder.Services.AddSingleton<IChatService, AgentChatService>();

var app = builder.Build();

// Single POST endpoint at /send that accepts a JSON payload of the form:
// { "request": "<request message>", "threadId": "<optional thread id>" }
app.MapPost("/send", async (SendRequest request, IChatService chatService) =>
{
    if (string.IsNullOrWhiteSpace(request.Request))
    {
        return Results.BadRequest(new { error = "The 'request' field is required." });
    }

    var threadId = request.ThreadId ?? string.Empty;

    ChatResult result = await chatService.PostMessage(request.Request, threadId);

    var response = new SendResponse
    {
        Response = result.Response,
        ThreadId = result.ThreadId
    };

    return Results.Ok(response);
});

//app.UseSwagger();
//app.UseSwaggerUI();

app.MapA2A(weatherAgent, path: "/a2a/weather", agentCard: new()
{
    Name = "Weather Agent",
    Description = "An agent that provides weather information using MCP tools.",
    Version = "1.0"
});

app.MapA2A(nflAgent, path: "/a2a/nfl", agentCard: new()
{
    Name = "NFL Agent",
    Description = "An agent that provides NFL information using MCP tools.",
    Version = "1.0"
});

app.Run();
