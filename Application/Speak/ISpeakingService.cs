using Domain.Models;
using Microsoft.Extensions.AI;

namespace Application.Speak;

/// <summary>
/// Responsible for speaking functionality, such as text-to-speech.
/// </summary>
public interface ISpeakingService
{
    /// <summary>
    /// Streams a response from a chat client.
    /// </summary>
    /// <param name="history">The current conversation history of chat messages.</param>
    /// <returns>An asynchronous collection of chat response updates from a chat client.</returns>
    IAsyncEnumerable<ChatResponseUpdate> StreamAsync(List<ChatMessage> history);
}
