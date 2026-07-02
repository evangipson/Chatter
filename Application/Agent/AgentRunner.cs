using Microsoft.Extensions.AI;
using Application.Context;
using Application.Tool;
using Domain.Constants;
using Domain.Events;
using Domain.Exceptions;
using Domain.Models;

namespace Application.Agent;

/// <inheritdoc cref="IAgentRunner"/>
public sealed class AgentRunner(IChatClient chatClient, IContextFactory contextFactory, ToolRegistry toolRegistry) : IAgentRunner
{
    public async IAsyncEnumerable<AgentEvent> RunAsync(ChatRequest chatRequest)
    {
        // begin the timer for reporting agent run duration
        var started = DateTime.UtcNow;

        // broacast that the agent has started to the user interface
        yield return new AgentStartedEvent();

        // create a local mutable copy of the optimized conversation context to fill up and broadcast back when the agent is finished
        List<ChatMessage> agentMessages = [.. await contextFactory.CreateAsync(chatRequest.ConversationId, chatRequest)];

        // create a fresh tool context and hook up the registered tools to the agent
        var toolContext = contextFactory.CreateToolContext(chatRequest.ConversationId, chatRequest.WorkspaceId.GetValueOrDefault(WorkspaceConstants.GlobalWorkspaceId));
        var tools = ToolFactory.Create(toolRegistry.Tools, toolContext);
        ChatOptions options = new() { Tools = [.. tools] };

        // while the agent has tools to run for a user's message, iterate and run each tool
        while (true)
        {
            // get a response from the chat client using the registered tools using chat options
            var response = await chatClient.GetResponseAsync(agentMessages, options);

            // add the messages that were received into the new local mutable copy of chat history
            agentMessages.AddRange(response.Messages);

            // if there are no tool calls, broadcast to the user interface that the agent is done and stop the agent
            if (!TryGetToolCalls(response, out var toolCalls))
            {
                yield return new AgentFinishedEvent(DateTime.UtcNow - started, agentMessages);
                yield break;
            }

            // attempt to call each agentic tool
            foreach (var call in toolCalls)
            {
                // try and get the intended tool, stop the chain if a tool is not found
                if (!toolRegistry.TryGet(call.Name, out var tool))
                {
                    agentMessages.Add(new(ChatRole.Tool, $"unknown tool \"{call.Name}\" called."));
                    yield return new AgentFinishedEvent(DateTime.UtcNow - started, agentMessages);
                    yield break;
                }

                // broadcast that a tool has started to the user interface
                yield return new ToolStartedEvent(call.Name);

                // wrap tool execution to allow agent recovery from errors
                var toolStarted = DateTime.UtcNow;
                string? result;
                try
                {
                    result = await tool.ExecuteAsync(toolContext, call.Arguments);
                }
                catch (ToolException toolException)
                {
                    result = toolException.Message;
                }

                // save the new message as chat from the tool
                agentMessages.Add(new(ChatRole.Tool, result));

                // broadcast that a tool has finished to the user interface
                yield return new ToolFinishedEvent(call.Name, DateTime.UtcNow - toolStarted);
            }
        }
    }

    private static bool TryGetToolCalls(ChatResponse? response, out IReadOnlyList<FunctionCallContent> toolCalls)
    {
        toolCalls = [..response?.Messages?.SelectMany(x => x.Contents).OfType<FunctionCallContent>() ?? []];
        return toolCalls.Count > 0;
    }
}