using System.Text;
using Application.Context;
using Application.Conversation;
using Application.Speak;
using Domain.Constants;
using Domain.Models;

namespace Application.Chat;

/// <inheritdoc cref="IChatService"/>
public class ChatService(IConversationService conversationService, IContextFactory contextFactory, ISpeakingService speakingService) : IChatService
{
    public async IAsyncEnumerable<string> RespondAsync(ChatRequest request, bool speak = false)
    {
        // if the request has no message, there is nothing to respond to
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            yield break;
        }

        // add the user's message to the conversation
        await conversationService.AddUserMessageAsync(request.ConversationId, request.Message, request.ImageBase64);

        // create an optimized conversation context
        var context = await contextFactory.CreateAsync(request.ConversationId, request);

        // create a fresh tool context
        var toolContext = contextFactory.CreateToolContext(request.ConversationId, request.WorkspaceId.GetValueOrDefault(WorkspaceConstants.GlobalWorkspaceId));

        // stream a response to the user's message
        StringBuilder responseBuffer = new();
        await foreach (var token in speakingService.StreamAndSpeakAsync(toolContext, context, speak))
        {
            responseBuffer.Append(token);
            yield return token;
        }

        // update the conversation context with the new chat message
        await conversationService.AddBotMessageAsync(request.ConversationId, responseBuffer.ToString());
    }
}