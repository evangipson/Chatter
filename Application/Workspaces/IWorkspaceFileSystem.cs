using Domain.Models;

namespace Application.Workspaces;

public interface IWorkspaceFileSystem
{
    Task<List<string>> GetFilesAsync(Guid workspaceId);

    Task<string> ReadFileAsync(Guid workspaceId, string relativePath);

    Task<List<string>> SearchFilesAsync(Guid workspaceId, string pattern);

    Task<List<TextSearchResult>> SearchTextAsync(Guid workspaceId, string text, int maxResults = 300);

    Task WriteFileAsync(Guid workspaceId, string relativePath, string contents);

    Task<CommandResult> RunCommandAsync(Guid workspaceId, string command, string arguments);
}
