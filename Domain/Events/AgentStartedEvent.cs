using Domain.Constants;

namespace Domain.Events;

/// <summary>
/// Represents an agent that is starting it's work.
/// </summary>
public sealed record AgentStartedEvent() : AgentEvent(Type: EventConstants.AgentStarted);