using Domain.Models;

namespace Application.Workspaces;

/// <summary>
/// Responsible for workspace functionality.
/// </summary>
public interface IWorkspaceService
{
    /// <summary>
    /// Creates a workspace with the provided <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the workspace to create.</param>
    /// <returns>An awaitable task that contains a <see langword="new"/> workspace.</returns>
    Task<Workspace> CreateAsync(string name);

    /// <summary>
    /// Gets a workspace with the provided <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The identifier of the desired workspace.</param>
    /// <returns>An awaitable task that contains a workspace that defaults to <see langword="null"/>.</returns>
    Task<Workspace?> GetAsync(Guid id);

    /// <summary>
    /// Gets all workspaces.
    /// </summary>
    /// <returns>An awaitable task that contains a collection of all workspaces.</returns>
    Task<List<Workspace>> GetAllAsync();
}