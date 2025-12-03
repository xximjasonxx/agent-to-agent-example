namespace Backend.Models;

/// <summary>
/// Represents the result of posting a message to the chat agent.
/// </summary>
public sealed record ChatResult(string Response, string ThreadId);
