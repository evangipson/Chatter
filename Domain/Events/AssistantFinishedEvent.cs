using Domain.Constants;

namespace Domain.Events;

/// <summary>
/// Represents an assistant that is finished broadcasting tokens.
/// </summary>
public sealed record AssistantFinishedEvent() : AgentEvent(Type: EventConstants.AssistantFinished);
