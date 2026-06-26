using Domain.Models;
using Microsoft.Extensions.AI;

namespace Application.Tool;

/// <summary>
/// A registry of all tools for the application.
/// </summary>
/// <param name="tools">A collection of all <see cref="ITool"/>.</param>
public sealed class ToolRegistry(ReadFileTool readFile, ListFilesTool listFiles, SearchTextTool searchText, WriteFileTool writeFile, RunCommandTool runCommand)
{
    /// <summary>
    /// Creates a collection of <see cref="AIFunction"/> for all the registered tools.
    /// </summary>
    /// <param name="context">The current tool context.</param>
    /// <returns>A collection of <see cref="AIFunction"/> for each registered tool.</returns>
    public IReadOnlyList<AIFunction> CreateFunctions(ToolContext context) =>
    [
        // TODO: COMMENT ENTRY POINTS OF EACH AI FUNCTION AND SEE IF THEY ARE RUNNING
        AIFunctionFactory.Create((string path) => readFile.Execute(context, path), name: "read_file", description: "Reads the contents of a file in the workspace."),
        AIFunctionFactory.Create(() => listFiles.Execute(context), name: "list_files", description: "Lists all files in the workspace."),
        AIFunctionFactory.Create((string text) => searchText.Execute(context, text), name: "search_text", description: "Searches for text inside files."),
        AIFunctionFactory.Create((string path, string contents) => writeFile.Execute(context, path, contents), name: "write_file", description: "Writes a file to the workspace."),
        AIFunctionFactory.Create((string command, string args) => runCommand.Execute(context, command, args), name: "run_command", description: "Runs a shell command in the workspace.")
    ];
}
