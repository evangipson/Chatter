using System.Text.Json;
using Application.Extensions;
using Application.Workspaces;
using Domain.Models;

namespace Application.Tool;

/// <inheritdoc cref="ITool"/>
public sealed class SearchTextTool(IWorkspaceFileSystem fs) : ITool
{
    public string Name => "search_text";

    public string Description => "Searches text for a match.";

    public async Task<string> ExecuteAsync(ToolContext context, IDictionary<string, object?>? arguments)
        => JsonSerializer.Serialize(await fs.SearchTextAsync(context.WorkspaceId, arguments.TryGetJsonArg("text")));
}
