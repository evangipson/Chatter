using System.Text.Json;
using Application.Workspaces;
using Domain.Constants;
using Domain.Models;

namespace Application.Tool;

/// <inheritdoc cref="ITool"/>
public sealed class ListFilesTool(IWorkspaceFileSystem fs) : ITool
{
    public string Name => "list_files";

    public string Description => "Lists files.";

    public async Task<string> ExecuteAsync(ToolContext context, IDictionary<string, object?>? _)
        => JsonSerializer.Serialize(await fs.GetFilesAsync(context.WorkspaceId), JsonConstants.DefaultSerializerOptions);
}
