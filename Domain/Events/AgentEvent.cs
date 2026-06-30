namespace Domain.Events;

/// <summary>
/// Represents an agentic event that is broadcasted to the user interface.
/// </summary>
/// <param name="Type">The type of agent event.</param>
public abstract record AgentEvent(string Type);