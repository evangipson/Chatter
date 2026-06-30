using Domain.Constants;

namespace Domain.Events;

/// <summary>
/// Represents an agent has finished a tool invocation.
/// </summary>
/// <param name="ToolName">The name of the tool that was started by the agent.</param>
/// <param name="Duration">The total duration of the tool invocation.</param>
public sealed record ToolFinishedEvent(string ToolName, TimeSpan Duration) : AgentEvent(EventType: EventConstants.ToolFinished);