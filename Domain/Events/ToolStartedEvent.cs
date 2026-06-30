using Domain.Constants;

namespace Domain.Events;

/// <summary>
/// Represents an agent has started a tool invocation.
/// </summary>
/// <param name="ToolName">The name of the tool that was started by the agent.</param>
public sealed record ToolStartedEvent(string ToolName) : AgentEvent(EventType: EventConstants.ToolStarted);