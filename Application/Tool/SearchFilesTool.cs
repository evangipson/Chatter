using System.Text.Json;
using Application.Extensions;
using Application.Workspaces;
using Domain.Models;

namespace Application.Tool;

/// <inheritdoc cref="ITool"/>
public sealed class SearchFilesTool(IWorkspaceFileSystem fs) : ITool
{
    public string Name => "search_files";

    public string Description => "Searches a workspace using a search pattern to locate a subset of files that match the search pattern.";

    public async Task<string> ExecuteAsync(ToolContext context, IDictionary<string, object?>? arguments)
        => JsonSerializer.Serialize(await fs.SearchFilesAsync(context.WorkspaceId, arguments.TryGetJsonArg("pattern")));
}
