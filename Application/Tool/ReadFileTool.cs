using Application.Workspaces;
using Domain.Models;

namespace Application.Tool;

public sealed class ReadFileTool(IWorkspaceFileSystem fs)
{
    public const string Name = "read_file";

    public const string Description = "Reads a file.";

    public async Task<string> Execute(ToolContext context, string path) => await fs.ReadFileAsync(context.WorkspaceId, path);
}