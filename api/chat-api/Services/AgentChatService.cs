using Backend.Models;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using System.Collections.Concurrent;

namespace Backend.Services;

/// <summary>
/// Implementation of <see cref="IChatService"/> that integrates AI chat with MCP tools.
/// </summary>
public sealed class AgentChatService : IChatService
{
    private readonly IChatClient _chatClient;
    private readonly ILogger<AgentChatService> _logger;
    private readonly ConcurrentDictionary<string, List<ChatMessage>> _conversations = new();
    private IList<AITool>? _tools;

    public AgentChatService(
        IChatClient chatClient,
        ILogger<AgentChatService> logger)
    {
        _chatClient = chatClient;
        _logger = logger;
    }

    public async Task<ChatResult> PostMessage(string message, string threadId = "")
    {
        try
        {
            // Generate or use existing thread ID
            var effectiveThreadId = string.IsNullOrWhiteSpace(threadId)
                ? Guid.NewGuid().ToString("N")
                : threadId;

            return new ChatResult(string.Empty, effectiveThreadId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message");
            throw;
        }
    }
}
