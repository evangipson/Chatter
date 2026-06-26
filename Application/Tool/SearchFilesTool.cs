using Application.Workspaces;
using Domain.Models;

namespace Application.Tool;

public sealed class SearchFilesTool(IWorkspaceFileSystem fs)
{
    public const string Name = "search_files";

    public const string Description = "Searches files.";

    public async Task<List<string>> Execute(ToolContext context, string pattern) => await fs.SearchFilesAsync(context.WorkspaceId, pattern);
}
