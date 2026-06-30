using Microsoft.Extensions.AI;
using Application.Tool;
using Domain.Models;
using Domain.Events;

namespace Application.Agent;

/// <inheritdoc cref="IAgentRunner"/>
public sealed class AgentRunner(IChatClient chatClient, ToolRegistry toolRegistry) : IAgentRunner
{
    public async IAsyncEnumerable<AgentEvent> RunAsync(ToolContext context, List<ChatMessage> history)
    {
        var started = DateTime.UtcNow;
        var messages = new List<ChatMessage>(history);
        var tools = ToolFactory.Create(toolRegistry.Tools, context);
        ChatOptions options = new() { Tools = [.. tools] };

        yield return new AgentStartedEvent();
        var iteration = 1;
        while (true)
        {
            Console.WriteLine($"Iteration {iteration++}");
            var response = await chatClient.GetResponseAsync(messages, options);
            Console.WriteLine("LLM returned.");
            messages.AddRange(response.Messages);

            if (!TryGetToolCalls(response, out var toolCalls))
            {
                yield return new AgentFinishedEvent(DateTime.UtcNow - started, messages);
                yield break;
            }

            foreach (var call in toolCalls)
            {
                if (!toolRegistry.TryGet(call.Name, out var tool))
                {
                    throw new InvalidOperationException($"unknown tool \"{call.Name}\" called.");
                }

                yield return new ToolStartedEvent(call.Name);

                var toolStarted = DateTime.UtcNow;
                var result = await tool.ExecuteAsync(context, call.Arguments);
                var duration = DateTime.UtcNow - toolStarted;
                messages.Add(new(ChatRole.Tool, result ?? string.Empty));

                yield return new ToolFinishedEvent(call.Name, duration);
            }
        }
    }

    private static bool TryGetToolCalls(ChatResponse? response, out IReadOnlyList<FunctionCallContent> toolCalls)
    {
        toolCalls = [..response?.Messages?.SelectMany(x => x.Contents).OfType<FunctionCallContent>() ?? []];
        return toolCalls.Count > 0;
    }
}