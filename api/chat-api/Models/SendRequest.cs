using System.Text.Json.Serialization;

namespace Backend.Models;

/// <summary>
/// Request payload for the /send endpoint.
/// </summary>
public sealed class SendRequest
{
    /// <summary>
    /// The message content from the user.
    /// </summary>
    [JsonPropertyName("request")]
    public string Request { get; set; } = string.Empty;

    /// <summary>
    /// Optional thread identifier used to continue an existing conversation.
    /// If omitted, a new thread will be created.
    /// </summary>
    [JsonPropertyName("threadId")]
    public string? ThreadId { get; set; }
}
