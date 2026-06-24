using Microsoft.Extensions.AI;
using Domain.Models;

namespace Application.Context;

/// <summary>
/// Responsible for creating conversation context.
/// </summary>
public interface IContextFactory
{
    /// <summary>
    /// Creates context for a conversation.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <param name="request">The chat request used to update conversation context.</param>
    /// <returns>An awaitable task containing the conversation context.</returns>
    Task<List<ChatMessage>> CreateAsync(Guid conversationId, ChatRequest request);
}
