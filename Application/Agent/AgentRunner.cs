using System.Text;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Application.Tool;
using Domain.Models;

namespace Application.Agent;

/// <inheritdoc cref="IAgentRunner"/>
public sealed class AgentRunner(ILogger<AgentRunner> logger, IChatClient chatClient, ToolRegistry toolRegistry) : IAgentRunner
{
    private const int _maxAgentIterations = 10;

    public async Task<AgentResult> RunAsync(ToolContext context, List<ChatMessage> history)
    {
        // begin the timer for the agent result
        var agentStartTime = DateTime.UtcNow;
        logger.LogInformation("starting agent runner (started at {AgentStartTime}).", agentStartTime);

        // hook up all registered tools to the agent
        var tools = toolRegistry.CreateFunctions(context);
        ChatOptions options = new() { Tools = [.. tools] };
        logger.LogInformation("{ToolCount} tools are registered to the agent.", tools.Count);

        // create a mutable copy of the current chat history
        var messages = new List<ChatMessage>(history);

        // iterate over tool calls until all tool calls are done
        StringBuilder finalText = new();
        List<ToolInvocation> toolInvocations = [];
        for (int i = 0; i < _maxAgentIterations; i++)
        {
            // capture assistant output
            logger.LogInformation("starting agent iteration #{Iteration}...", i + 1);
            var response = await chatClient.GetResponseAsync(messages, options);
            if (!string.IsNullOrWhiteSpace(response.Text))
            {
                finalText.Append(response.Text);
            }

            // try and get the tool calls for this iteration, early exit if there aren't any
            if (!TryGetToolCalls(response, out var toolCalls))
            {
                logger.LogInformation("there were no tool calls in the response (duration: {Duration})", DateTime.UtcNow - agentStartTime);
                return new(response: finalText.ToString(), duration: DateTime.UtcNow - agentStartTime);
            }

            // execute each tool in the response and add the results to the agent response and chat messages
            foreach (var toolResultTask in toolCalls.Select(x => GetToolCallResult(tools, x)))
            {
                var toolResult = await toolResultTask;
                logger.LogInformation("tool \"{ToolName}\" result\n{ToolResult}\n", toolResult.ToolName, toolResult.Result);
                toolInvocations.Add(toolResult);
                messages.Add(new(ChatRole.Tool, toolResult.Result));
            }
        }

        // return the final agent result which has all tool call results
        logger.LogInformation("agent runner done (finished in {Duration})", DateTime.UtcNow - agentStartTime);
        return new(response: finalText.ToString(), duration: DateTime.UtcNow - agentStartTime, toolCallResults: toolInvocations);
    }

    private static bool TryGetToolCalls(ChatResponse? response, out IReadOnlyList<FunctionCallContent> toolCalls)
    {
        toolCalls = [..response?.Messages?.SelectMany(x => x.Contents).OfType<FunctionCallContent>() ?? []];
        return toolCalls.Count > 0;
    }

    private static async Task<ToolInvocation> GetToolCallResult(IReadOnlyList<AIFunction> tools, FunctionCallContent toolCall)
    {
        var toolStartTime = DateTime.UtcNow;
        var result = await tools.First(t => t.Name == toolCall.Name).InvokeAsync(new(toolCall.Arguments));
        return new(toolCall.Name, DateTime.UtcNow - toolStartTime, result?.ToString(), string.Join(", ", toolCall.Arguments?.Keys ?? []), result != null);
    }
}