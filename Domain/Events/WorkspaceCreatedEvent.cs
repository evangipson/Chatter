using Domain.Constants;

namespace Domain.Events;

/// <summary>
/// Represents that a workspace has been created by an agent.
/// </summary>
/// <param name="WorkspaceId">The identifier of the workspace created by an agent.</param>
public sealed record WorkspaceCreatedEvent(Guid WorkspaceId) : AgentEvent(EventType: EventConstants.WorkspaceCreated);