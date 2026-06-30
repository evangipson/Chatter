using Microsoft.Extensions.AI;

namespace Application.Speak;

/// <inheritdoc cref="ISpeakingService"/>
public class SpeakingService(IChatClient chatClient) : ISpeakingService
{
    public async IAsyncEnumerable<ChatResponseUpdate> StreamAsync(List<ChatMessage> history)
    {
        await foreach (var update in chatClient.GetStreamingResponseAsync(history))
        {
            yield return update;
        }
    }
}
