using Backend.Models;

namespace Backend.Services;

/// <summary>
/// Abstraction for posting chat messages to an AI-backed agent.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Posts a chat message to the agent and returns the agent's response and thread id.
    /// </summary>
    /// <param name="message">The user message to send.</param>
    /// <param name="threadId">
    /// Optional thread identifier used to resume an existing conversation.
    /// If not supplied, an empty string should be passed and a new thread id may be generated.
    /// </param>
    /// <returns>The chat result containing the response text and thread id.</returns>
    Task<ChatResult> PostMessage(string message, string threadId = "");
}
