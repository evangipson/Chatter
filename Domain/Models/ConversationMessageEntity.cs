namespace Domain.Models;

/// <summary>
/// Represents a conversation message.
/// <para>
/// A collection is persisted in the <c>"Messages"</c> table in the database.
/// </para>
/// </summary>
public class ConversationMessageEntity
{
    /// <summary>
    /// The identifier of the message.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The identifier of the associated conversation.
    /// </summary>
    public Guid ConversationId { get; set; }

    /// <summary>
    /// The sender's conversational role.
    /// </summary>
    public string Role { get; set; } = "";

    /// <summary>
    /// The content of the message.
    /// </summary>
    public string Content { get; set; } = "";

    /// <summary>
    /// The time the message happened.
    /// </summary>
    public DateTime CreatedUtc { get; set; }
}
