namespace Domain.Models;

/// <summary>
/// Represents a workspace, or a directory filled with directories and files.
/// <para>
/// A collection is persisted in the <c>"Workspaces"</c> table in the database.
/// </para>
/// </summary>
public class Workspace
{
    /// <summary>
    /// The identifier of a workspace.
    /// <para>
    /// When there is no specific workspace, defaults to <see cref="Constants.WorkspaceConstants.GlobalWorkspaceId"/>.
    /// </para>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of a workspace.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// The root path of the top-level directory of the workspace.
    /// </summary>
    public string RootPath { get; set; } = "";

    /// <summary>
    /// The time that the workspace was created.
    /// </summary>
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// The last time the workspace was modified.
    /// </summary>
    public DateTime LastModifiedUtc { get; set; }
}
