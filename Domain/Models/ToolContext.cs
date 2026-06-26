namespace Domain.Models;

/// <summary>
/// Necessary context for running a chain of tools.
/// </summary>
/// <param name="workspaceId">The identifier of the current workspace.</param>
/// <param name="converationId">The identifier of the current conversation.</param>
public readonly struct ToolContext(Guid workspaceId, Guid converationId)
{
    /// <summary>
    /// The identifier of the current workspace.
    /// </summary>
    public readonly Guid WorkspaceId = workspaceId;

    /// <summary>
    /// The identifier of the current conversation.
    /// </summary>
    public readonly Guid ConversationId = converationId;
}
