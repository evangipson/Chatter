namespace Application.Conversation;

/// <summary>
/// Responsible for conversation functionality.
/// </summary>
public interface IConversationService
{
    /// <summary>
    /// Adds a user message to a conversation.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <param name="message">The user's message to add to the conversation.</param>
    /// <param name="imageBase64">The optional base64 image sent with the user's message.</param>
    /// <returns>An awaitable task.</returns>
    Task AddUserMessageAsync(Guid conversationId, string message, string? imageBase64 = null);

    /// <summary>
    /// Adds a bot response to a conversation.
    /// <para>
    /// Also updates the conversation context.
    /// </para>
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <param name="message">The bot response to add to the conversation.</param>
    /// <returns>An awaitable task.</returns>
    Task AddBotMessageAsync(Guid conversationId, string message);
}
