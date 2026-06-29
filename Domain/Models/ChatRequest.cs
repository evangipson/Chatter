namespace Domain.Models;

/// <summary>
/// Represents a chat request to the agent.
/// </summary>
public class ChatRequest
{
    /// <summary>
    /// The identifier for the chat's conversation.
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// The identifier of the associated workspace.
    /// <para>
    /// When there is no specific workspace, defaults to <see cref="Constants.WorkspaceConstants.GlobalWorkspaceId"/>.
    /// </para>
    /// </summary>
    public Guid? WorkspaceId { get; set; }

    /// <summary>
    /// The contents of the chat message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// The base64 representation of any images associated with a chat message.
    /// </summary>
    public string? ImageBase64 { get; init; }

    /// <summary>
    /// The system prompt of the chat.
    /// <para>
    /// Used when beginning a conversation to set the "tone".
    /// </para>
    /// </summary>
    public string? SystemPrompt { get; set; }
}