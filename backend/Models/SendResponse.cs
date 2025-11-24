using System.Text.Json.Serialization;

namespace Backend.Models;

/// <summary>
/// Response payload for the /send endpoint.
/// </summary>
public sealed class SendResponse
{
    /// <summary>
    /// The response text produced by the AI model.
    /// </summary>
    [JsonPropertyName("response")]
    public string Response { get; set; } = string.Empty;

    /// <summary>
    /// The thread identifier associated with this conversation.
    /// </summary>
    [JsonPropertyName("threadId")]
    public string ThreadId { get; set; } = string.Empty;
}
