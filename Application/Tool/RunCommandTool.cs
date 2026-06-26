using System.Text.Json;
using Application.Workspaces;
using Domain.Models;

namespace Application.Tool;

public sealed class RunCommandTool(IWorkspaceFileSystem fs)
{
    public const string Name = "run_command";

    public const string Description = "Runs a command.";

    public async Task<CommandResult> Execute(ToolContext context, string command, string arguments)
        => await fs.RunCommandAsync(context.WorkspaceId, command, arguments);
}
