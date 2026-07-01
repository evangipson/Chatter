using System.Text.Json;
using Application.Extensions;
using Application.Workspaces;
using Domain.Constants;
using Domain.Models;

namespace Application.Tool;

/// <inheritdoc cref="ITool"/>
public sealed class RunCommandTool(IWorkspaceFileSystem fs) : ITool
{
    public string Name => "run_command";

    public string Description => "Runs a command in a sandboxed execution context, typically used for things like building and running tests.";

    public async Task<string> ExecuteAsync(ToolContext context, IDictionary<string, object?>? arguments)
    {
        var result = await fs.RunCommandAsync(context.WorkspaceId, arguments.TryGetJsonArg("command"), arguments.TryGetJsonArg("arguments"));
        return JsonSerializer.Serialize(result, JsonConstants.DefaultSerializerOptions);
    }
}
