using Backend.Models;
using Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Register application services
builder.Services.AddScoped<IChatService, AgentChatService>();

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

app.Run();
