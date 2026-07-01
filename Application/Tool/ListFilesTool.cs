using System.Text.Json;
using Application.Workspaces;
using Domain.Constants;
using Domain.Models;

namespace Application.Tool;

/// <inheritdoc cref="ITool"/>
public sealed class ListFilesTool(IWorkspaceFileSystem fs) : ITool
{
    public string Name => "list_files";

    public string Description => "Lists all files of a workspace; if you know a directory or subset of files you'd like to see instead, use the `search_files` tool first.";

    public async Task<string> ExecuteAsync(ToolContext context, IDictionary<string, object?>? _)
        => JsonSerializer.Serialize(await fs.GetFilesAsync(context.WorkspaceId), JsonConstants.DefaultSerializerOptions);
}
