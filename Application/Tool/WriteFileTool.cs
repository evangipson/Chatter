using Application.Extensions;
using Application.Workspaces;
using Domain.Models;

namespace Application.Tool;

/// <inheritdoc cref="ITool"/>
public sealed class WriteFileTool(IWorkspaceFileSystem fs) : ITool
{
    public string Name => "write_file";

    public string Description => "Writes to a file.";

    public async Task<string> ExecuteAsync(ToolContext context, IDictionary<string, object?>? arguments)
    {
        var path = arguments.TryGetJsonArg("path");
        await fs.WriteFileAsync(context.WorkspaceId, path, arguments.TryGetJsonArg("contents"));
        return $"The file \"{path}\" was written to successfully.";
    }
}