using Microsoft.Extensions.AI;
using Domain.Models;

namespace Application.Agent;

/// <summary>
/// Represents the results of an agent run.
/// </summary>
/// <param name="History">The history of chat messages from an agent run.</param>
/// <param name="ToolInvocations">A <see langword="readonly"/> collection of all tool invocations from an agent run.</param>
/// <param name="Duration">The total time of an agent run.</param>
public readonly record struct AgentRunnerResult(List<ChatMessage> History, IReadOnlyList<ToolInvocation> ToolInvocations, TimeSpan Duration);