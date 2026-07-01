using Domain.Models;

namespace Application.Workspaces;

/// <summary>
/// Responsible for file system functionality for workspaces.
/// </summary>
public interface IWorkspaceFileSystem
{
    /// <summary>
    /// Gets files for the current workspace.
    /// </summary>
    /// <param name="workspaceId">The identifier for the current workspace.</param>
    /// <returns>An awaitable task containing a collection of file paths.</returns>
    Task<List<string>> GetFilesAsync(Guid workspaceId);

    /// <summary>
    /// Reads a file from the current workspace.
    /// <para>
    /// Will <see langword="throw"/> when the thread it's running on has been canceled.
    /// </para>
    /// </summary>
    /// <param name="workspaceId">The identifier for the current workspace.</param>
    /// <param name="relativePath">The relative path of the file to read.</param>
    /// <returns>An awaitable task containing content for a file.</returns>
    /// <exception cref="OperationCanceledException"/>
    Task<string> ReadFileAsync(Guid workspaceId, string relativePath);

    /// <summary>
    /// Searches files from the current workspace.
    /// </summary>
    /// <param name="workspaceId">The identifier for the current workspace.</param>
    /// <param name="pattern">The pattern to use when searching for files.</param>
    /// <returns>An awaitable task containing a list of all the files that match <paramref name="pattern"/>.</returns>
    Task<List<string>> SearchFilesAsync(Guid workspaceId, string pattern);

    /// <summary>
    /// Searches content for <paramref name="text"/> in the current workspace.
    /// </summary>
    /// <param name="workspaceId">The identifier for the current workspace.</param>
    /// <param name="text">The search pattern for the content.</param>
    /// <param name="maxResults">The maximum number of results returned, defaults to <c>300</c>.</param>
    /// <returns>An awaitable task containing a collection of text search results.</returns>
    Task<List<TextSearchResult>> SearchTextAsync(Guid workspaceId, string text, int maxResults = 300);

    /// <summary>
    /// Writes a file to the current workspace.
    /// <para>
    /// Will <see langword="throw"/> when the application doesn't have write permission in a workspace.
    /// </para>
    /// </summary>
    /// <param name="workspaceId">The identifier for the current workspace.</param>
    /// <param name="relativePath">The relative file path for the new file.</param>
    /// <param name="contents">The contents to write to the file.</param>
    /// <returns>An awaitable task.</returns>
    /// <exception cref="UnauthorizedAccessException"/>
    Task WriteFileAsync(Guid workspaceId, string relativePath, string contents);

    /// <summary>
    /// Runs a command in the current workspace.
    /// </summary>
    /// <param name="workspaceId">The identifier for the current workspace.</param>
    /// <param name="command">The command to run (i.e.: <c>dotnet build</c>).</param>
    /// <param name="arguments">The arguments provided to the command.</param>
    /// <returns>An awaitable task containing the results of the command.</returns>
    Task<CommandResult> RunCommandAsync(Guid workspaceId, string command, string arguments);
}