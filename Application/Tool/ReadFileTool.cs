using Application.Extensions;
using Application.Workspaces;
using Domain.Models;

namespace Application.Tool;

/// <inheritdoc cref="ITool"/>
public sealed class ReadFileTool(IWorkspaceFileSystem fs) : ITool
{
    public string Name => "read_file";

    public string Description => "Reads a file.";

    public Task<string> ExecuteAsync(ToolContext context, IDictionary<string, object?>? arguments)
        => fs.ReadFileAsync(context.WorkspaceId, arguments.TryGetJsonArg("path"));
}