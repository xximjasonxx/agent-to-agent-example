using Backend.Models;
using ModelContextProtocol;

namespace Backend.Services;

/// <summary>
/// Default implementation of <see cref="IChatService"/> that currently echoes
/// the user message. Replace the implementation with real AI model integration.
/// </summary>
public sealed class AgentChatService : IChatService
{
    public async Task<ChatResult> PostMessage(string message, string threadId = "")
    {
        // In a real implementation, call your AI model or agent here and manage
        // conversation state keyed by threadId. For now, this simply echoes back
        // the incoming message and generates a thread id when none is supplied.

        var effectiveThreadId = string.IsNullOrWhiteSpace(threadId)
            ? Guid.NewGuid().ToString("N")
            : threadId;

        var responseText = $"Echo: {message}";

        return new ChatResult(responseText, effectiveThreadId);
    }
}
