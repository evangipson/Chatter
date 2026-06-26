using Microsoft.Extensions.AI;
using Domain.Models;

namespace Application.Repositories;

/// <summary>
/// Responsible for persisting conversations to the database.
/// </summary>
public interface IConversationRepository
{
    /// <summary>
    /// Gets a collection of conversations for a workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace identifier.</param>
    /// <returns>An awaitable task containing a collection of conversations.</returns>
    Task<List<ConversationEntity>> GetByWorkspaceId(Guid workspaceId);

    /// <summary>
    /// Gets a conversation.
    /// </summary>
    /// <param name="id">The conversation identifier.</param>
    /// <returns>An awaitable task that contains the conversation, defaults to <see langword="null"/>.</returns>
    Task<ConversationEntity?> GetAsync(Guid id);

    /// <summary>
    /// Creates a conversation for a workspace.
    /// </summary>
    /// <param name="workspaceId">The workspace identifier.</param>
    /// <returns>An awaitable task that contains the conversation.</returns>
    Task<ConversationEntity> CreateAsync(Guid workspaceId);

    /// <summary>
    /// Deletes a conversation.
    /// </summary>
    /// <param name="id">The conversation identifier.</param>
    /// <returns>An awaitable task.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Adds a message to a conversation.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <param name="message">The message to add to the conversation.</param>
    /// <returns>An awaitable task.</returns>
    Task AddMessageAsync(Guid conversationId, ChatMessage message);

    /// <summary>
    /// Gets all messages for a conversation.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <returns>An awaitable task containing a collection of conversation messages.</returns>
    Task<List<ChatMessage>> GetMessagesAsync(Guid conversationId);

    /// <summary>
    /// Updates the title of a conversation.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <param name="title">The new title of the conversation.</param>
    /// <returns>An awaitable task.</returns>
    Task UpdateTitleAsync(Guid conversationId, string title);

    /// <summary>
    /// Updates the summary of a conversation.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <param name="summary">The new summary of the conversation.</param>
    /// <returns>An awaitable task.</returns>
    Task UpdateSummaryAsync(Guid conversationId, string summary);

    /// <summary>
    /// Updates the last updated time of a conversation.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <returns>An awaitable task.</returns>
    Task TouchAsync(Guid conversationId);
}
