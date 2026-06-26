using Application.Workspaces;
using Domain.Models;

namespace Application.Tool;

public sealed class SearchTextTool(IWorkspaceFileSystem fs)
{
    public const string Name = "search_text";

    public const string Description = "Searches text for a match.";

    public async Task<List<TextSearchResult>> Execute(ToolContext context, string text) => await fs.SearchTextAsync(context.WorkspaceId, text);
}
