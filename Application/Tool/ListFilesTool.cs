using Application.Workspaces;
using Domain.Models;

namespace Application.Tool;

public sealed class ListFilesTool(IWorkspaceFileSystem fs)
{
    public const string Name = "list_files";

    public const string Description = "Lists files.";

    public async Task<List<string>> Execute(ToolContext context) => await fs.GetFilesAsync(context.WorkspaceId);
}
