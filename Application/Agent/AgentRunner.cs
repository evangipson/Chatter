using Microsoft.Extensions.AI;
using Application.Tool;
using Domain.Events;
using Domain.Exceptions;
using Domain.Models;

namespace Application.Agent;

/// <inheritdoc cref="IAgentRunner"/>
public sealed class AgentRunner(IChatClient chatClient, ToolRegistry toolRegistry) : IAgentRunner
{
    public async IAsyncEnumerable<AgentEvent> RunAsync(ToolContext context, List<ChatMessage> history)
    {
        // begin the timer for reporting agent run duration
        var started = DateTime.UtcNow;

        // broacast that the agent has started to the user interface
        yield return new AgentStartedEvent();

        // create a local mutable copy of the chat history
        var messages = new List<ChatMessage>(history);
        
        // hook up the registered tools to the agent using chat options
        var tools = ToolFactory.Create(toolRegistry.Tools, context);
        ChatOptions options = new() { Tools = [.. tools] };

        // while the agent has tools to run for a user's message, iterate and run each tool
        while (true)
        {
            // get a response from the chat client using the tool chain (i.e.: the "agent")
            var response = await chatClient.GetResponseAsync(messages, options);

            // add the messages that were received into the new local mutable copy of chat history
            messages.AddRange(response.Messages);

            // if there are no tool calls, broadcast to the user interface that the agent is done and stop the agent
            if (!TryGetToolCalls(response, out var toolCalls))
            {
                yield return new AgentFinishedEvent(DateTime.UtcNow - started, messages);
                yield break;
            }

            // attempt to call each agentic tool
            foreach (var call in toolCalls)
            {
                // try and get the intended tool, stop the chain if a tool is not found
                if (!toolRegistry.TryGet(call.Name, out var tool))
                {
                    messages.Add(new(ChatRole.Tool, $"unknown tool \"{call.Name}\" called."));
                    yield return new AgentFinishedEvent(DateTime.UtcNow - started, messages);
                    yield break;
                }

                // broadcast that a tool has started to the user interface
                yield return new ToolStartedEvent(call.Name);

                // wrap tool execution to allow agent recovery from errors
                var toolStarted = DateTime.UtcNow;
                string? result;
                try
                {
                    result = await tool.ExecuteAsync(context, call.Arguments);
                }
                catch (ToolException toolException)
                {
                    result = toolException.Message;
                }

                // save the new message as chat from the tool
                messages.Add(new(ChatRole.Tool, result));

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