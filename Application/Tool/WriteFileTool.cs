using Application.Extensions;
using Application.Workspaces;
using Domain.Exceptions;
using Domain.Models;

namespace Application.Tool;

/// <inheritdoc cref="ITool"/>
public sealed class WriteFileTool(IWorkspaceFileSystem fs) : ITool
{
    public string Name => "write_file";

    public string Description => "Writes to a new file, or updates and existing file; if the file cannot be located, use the `search_files` tool first.";

    public async Task<string> ExecuteAsync(ToolContext context, IDictionary<string, object?>? arguments)
    {
        var path = arguments.TryGetJsonArg("path");
        try
        {
            await fs.WriteFileAsync(context.WorkspaceId, path, arguments.TryGetJsonArg("contents"));
            return $"Successfully wrote \"{path}\".";
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new ToolException($"Unable to write \"{path}\": {ex.Message}\nTell the user to make sure to check permissions for the workspace.");
        }
    }
}