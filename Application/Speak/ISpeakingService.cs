using Domain.Models;
using Microsoft.Extensions.AI;

namespace Application.Speak;

/// <summary>
/// Responsible for speaking functionality, such as text-to-speech.
/// </summary>
public interface ISpeakingService
{
    /// <summary>
    /// Streams a response from a chat client, optionally with text-to-speech.
    /// </summary>
    /// <param name="toolContext">The current tool context.</param>
    /// <param name="context">The conversation context.</param>
    /// <param name="speak">A flag that, when <see langword="true"/>, denotes the response will be spoken.</param>
    /// <returns>An asynchronous collection of response tokens from a chat client.</returns>
    IAsyncEnumerable<string> StreamAndSpeakAsync(ToolContext toolContext, List<ChatMessage> context, bool speak);
}
