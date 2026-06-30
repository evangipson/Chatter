using Domain.Constants;

namespace Domain.Events;

/// <summary>
/// Represents an assistant that is starting to broadcast tokens.
/// </summary>
public sealed record AssistantStartedEvent() : AgentEvent(EventType: EventConstants.AssistantStarted);