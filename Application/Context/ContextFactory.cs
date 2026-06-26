using Microsoft.Extensions.AI;
using Application.Repositories;
using Domain.Models;
using Domain.Constants;

namespace Application.Context;

/// <inheritdoc cref="IContextFactory"/>
public class ContextFactory(IConversationRepository conversationRepository) : IContextFactory
{
    public async Task<List<ChatMessage>> CreateAsync(Guid conversationId, ChatRequest request)
    {
        // build up the basic system prompt first
        var conversation = await conversationRepository.GetAsync(conversationId);
        var messages = await conversationRepository.GetMessagesAsync(conversationId);
        List<ChatMessage> context = [new(ChatRole.System, request.SystemPrompt)];

        // summarize the conversation using compressed memory and add it to the context
        if (!string.IsNullOrWhiteSpace(conversation?.Summary))
        {
            context.Add(new(ChatRole.System, $"Conversation Summary:\n{conversation.Summary}"));
        }

        // if there was an image on the request, add it as a temporary stop-gap
        // TODO: actually persist images in the DB with a messageID under conversation alongside ConversationMessageEntity
        var orderedMessages = messages.OrderBy(m => m.CreatedAt).TakeLast(ContextConstants.WindowSize);
        if (!string.IsNullOrWhiteSpace(request.ImageBase64))
        {
            ReadOnlyMemory<byte> imageBytes = Convert.FromBase64String(request.ImageBase64.Split(',')[1]);
            var imageType = request.ImageBase64[5..request.ImageBase64.IndexOf(';')];
            orderedMessages.LastOrDefault()?.Contents.Add(new DataContent(imageBytes, imageType));
        }

        // create a recent window from the last N messages and add it to the context
        context.AddRange(orderedMessages);

        return [..context];
    }

    public ToolContext CreateToolContext(Guid conversationId, Guid workspaceId) => new(workspaceId, conversationId);
}
