namespace Domain.Models;

/// <summary>
/// Represents a conversation.
/// <para>
/// A collection is persisted in the <c>"Conversations"</c> table in the database.
/// </para>
/// </summary>
public class ConversationEntity
{
    /// <summary>
    /// The identifier of the conversation.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The identifier of the associated workspace.
    /// <para>
    /// When there is no specific workspace, defaults to <see cref="Constants.WorkspaceConstants.GlobalWorkspaceId"/>.
    /// </para>
    /// </summary>
    public Guid WorkspaceId { get; set; }

    /// <summary>
    /// The title of the conversation, defaults to <c>"New Chat"</c>.
    /// </summary>
    public string Title { get; set; } = "New Chat";

    /// <summary>
    /// The time the conversation was created at.
    /// </summary>
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The time the conversation was last updated.
    /// </summary>
    public DateTime LastUpdatedUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The summary of the conversation, populated after message count exceeds <see cref="Constants.ConversationConstants.SummaryWindowSize"/>.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// The total number of messages in the conversation.
    /// <para>
    /// TODO: This is currently unused, but will be populated soon.
    /// </para>
    /// </summary>
    public int MessageCount { get; set; }
}
