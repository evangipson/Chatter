using System.Text;
using Microsoft.Extensions.AI;
using Application.Agent;
using Application.Context;
using Application.Conversation;
using Application.Speak;
using Domain.Constants;
using Domain.Events;
using Domain.Models;

namespace Application.Chat;

/// <inheritdoc cref="IChatService"/>
public class ChatService(IConversationService conversationService, IContextFactory contextFactory, IAgentRunner agentRunner, ISpeakingService speakingService) : IChatService
{
    public async IAsyncEnumerable<AgentEvent> RespondAsync(ChatRequest request, bool speak = false)
    {
        // if the request has no message, there is nothing to respond to
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            yield break;
        }

        // add the user's message to the conversation
        await conversationService.AddUserMessageAsync(request.ConversationId, request.Message, request.ImageBase64);

        // create an optimized conversation context and a fresh tool context for the agent
        var context = await contextFactory.CreateAsync(request.ConversationId, request);
        var toolContext = contextFactory.CreateToolContext(request.ConversationId, request.WorkspaceId.GetValueOrDefault(WorkspaceConstants.GlobalWorkspaceId));

        // stream a response to the user's message
        List<ChatMessage>? expandedHistory = null;
        await foreach (var agentEvent in agentRunner.RunAsync(toolContext, context))
        {
            if (agentEvent is AgentFinishedEvent completed)
            {
                expandedHistory = completed.History;
            }

            yield return agentEvent;
        }

        // exit the agent chat if no agent messages were generated
        if (expandedHistory is null)
        {
            yield break;
        }

        // broadcast that the assistant will start streaming tokens to the user interface
        yield return new AssistantStartedEvent();

        // generate the agent's response in a streaming fashion and broadcast each token
        StringBuilder finalResponse = new();
        await foreach (var update in speakingService.StreamAsync(expandedHistory))
        {
            finalResponse.Append(update.Text);
            yield return new AssistantTokenEvent(Text: update.Text);
        }

        // update the conversation context with the new chat message
        await conversationService.AddBotMessageAsync(request.ConversationId, finalResponse.ToString());

        // broadcast that the assistant is done streaming tokens to the user interface
        yield return new AssistantFinishedEvent();
    }
}