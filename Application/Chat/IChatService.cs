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
    /// <param name="speak">A flag that, when <see langword="true"/>, denotes the response will be spoken.</param>
    /// <returns>An asynchronous collection of response tokens from a chat client.</returns>
    IAsyncEnumerable<string> RespondAsync(ChatRequest request, bool speak = false);
}
