using Domain.Constants;

namespace Domain.Events;

/// <summary>
/// Represents an assistant token being broadcast to the user interface.
/// </summary>
/// <param name="Text">The content of the assistant token.</param>
public sealed record AssistantTokenEvent(string Text) : AgentEvent(Type: EventConstants.AssistantToken);