using System.Text;
using Microsoft.Extensions.AI;
using Application.Clients;
using Application.Repositories;
using Domain.Constants;
using Domain.Models;

namespace Application.Conversation;

/// <inheritdoc cref="IConversationService"/>
public class ConversationService(IConversationRepository conversationRepository) : IConversationService
{
    private static readonly ChatMessage TitlePrompt = new(ChatRole.System, ChatConstants.TitlePrompt);

    public Task AddUserMessageAsync(Guid conversationId, string message, string? imageBase64 = null)
    {
        // create a new chat message with the user's message
        ChatMessage userMessage = new(ChatRole.User, message);
        
        // add any image that was provided by the user
        if (!string.IsNullOrWhiteSpace(imageBase64))
        {
            ReadOnlyMemory<byte> imageBytes = Convert.FromBase64String(imageBase64.Split(',')[1]);
            var imageType = imageBase64[5..imageBase64.IndexOf(';')];
            userMessage.Contents.Add(new DataContent(imageBytes, imageType));
        }

        // add the user chat message to the conversation
        return AddChatMessageToConversation(conversationId, userMessage);
    }

    public async Task AddBotMessageAsync(Guid conversationId, string message)
    {
        // add the last context message to the conversation repository
        await AddChatMessageToConversation(conversationId, new(ChatRole.Assistant, message));

        // get the conversation from the repository, early exit if it's not there
        var conversation = await conversationRepository.GetAsync(conversationId);
        if (conversation == null)
        {
            return;
        }

        // trigger a thread for each conversation update
        await MaybeTitleAsync(conversationId, conversation);
        await MaybeSummarizeAsync(conversationId, conversation);
        await SetLastUpdatedAsync(conversationId, conversation);
    }

    private async Task AddChatMessageToConversation(Guid conversationId, ChatMessage chatMessage)
    {
        // if there is no text in the message, there is nothing to add
        if (string.IsNullOrWhiteSpace(chatMessage?.Text))
        {
            return;
        }

        // add the new message to the context and persist the new message to the conversation repository
        await conversationRepository.AddMessageAsync(conversationId, chatMessage);
    }

    private async Task MaybeTitleAsync(Guid conversationId, ConversationEntity conversation)
    {
        // if the title has already been set, don't set it again
        if (!string.IsNullOrWhiteSpace(conversation.Title) && conversation.Title != "New Chat")
        {
            return;
        }

        // get all conversation messages, early exit if there aren't enough messages for a summary
        var messages = await conversationRepository.GetMessagesAsync(conversationId);
        if (messages.Count == 0)
        {
            return;
        }

        // ask for a conversation title using a prompt
        List<ChatMessage> titleMessages = messages.Count >= ConversationConstants.TitleWindowSize
            ? [.. messages.Take(ConversationConstants.TitleWindowSize)]
            : [.. messages];
        var titleResponse = await ChatClients.Client.GetResponseAsync([TitlePrompt, ..titleMessages]);
        var title = titleResponse.Text?.Trim() ?? "Untitled Chat";

        // update the title of the conversation and persist the new title in the conversation repository
        conversation.Title = title;
        await conversationRepository.UpdateTitleAsync(conversationId, title);
    }

    private async Task MaybeSummarizeAsync(Guid conversationId, ConversationEntity conversation)
    {
        // get all conversation messages, early exit if there aren't enough messages for a summary
        var messages = await conversationRepository.GetMessagesAsync(conversationId);
        if (messages.Count < ConversationConstants.ContextWindowSize)
        {
            return;
        }

        // generate a summary for the conversation, early exit if one isn't created
        var summary = await GenerateSummaryAsync([..messages.Take(messages.Count - ConversationConstants.ContextWindowSize)]);
        if (string.IsNullOrWhiteSpace(summary))
        {
            return;
        }

        // update the summary of the conversation and persist the new summary in the conversation repository
        conversation.Summary = summary;
        await conversationRepository.UpdateSummaryAsync(conversationId, summary);
    }

    private static async Task<string> GenerateSummaryAsync(List<ChatMessage> messages)
    {
        // build a prompt to generate a summary
        var transcript = string.Join("\n", messages.Select(m => $"{m.Role}: {m.Text}"));
        List<ChatMessage> prompt = [new(ChatRole.System, ConversationConstants.SummaryPrompt), new(ChatRole.User, transcript)];

        // generate a summary from the chat client
        StringBuilder summaryBuffer = new();
        await foreach (var chunk in ChatClients.Client.GetStreamingResponseAsync(prompt))
        {
            summaryBuffer.Append(chunk.Text);
        }

        // return the newly-created summary
        return summaryBuffer.ToString();
    }

    private async Task SetLastUpdatedAsync(Guid conversationId, ConversationEntity conversation)
    {
        // update the last updated time of the conversation and persist the new time in the conversation repository
        conversation.LastUpdatedUtc = DateTime.UtcNow;
        await conversationRepository.TouchAsync(conversationId);
    }
}
