using Domain.Events;
using Domain.Models;

namespace Application.Chat;

/// <summary>
/// Responsible for orchestrating chat responses and maintaining conversation context.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Responds to a chat request and updates conversation context.
    /// </summary>
    /// <param name="request">The chat request to respond to.</param>
    /// <returns>An asynchronous collection of agent events from a chat client.</returns>
    IAsyncEnumerable<AgentEvent> RespondAsync(ChatRequest request);
}
