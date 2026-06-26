using Application.Workspaces;
using Domain.Models;

namespace Application.Tool;

public sealed class WriteFileTool(IWorkspaceFileSystem fs)
{
    public const string Name = "write_file";

    public const string Description = "Writes to a file.";

    public async Task<string> Execute(ToolContext context, string path, string contents)
    {
        await fs.WriteFileAsync(context.WorkspaceId, path, contents);
        return $"The file \"{path}\" was written to successfully.";
    }
}