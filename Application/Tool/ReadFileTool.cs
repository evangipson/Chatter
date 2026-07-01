using Application.Extensions;
using Application.Workspaces;
using Domain.Exceptions;
using Domain.Models;

namespace Application.Tool;

/// <inheritdoc cref="ITool"/>
public sealed class ReadFileTool(IWorkspaceFileSystem fs) : ITool
{
    public string Name => "read_file";

    public string Description => "Reads a file using an exact relative path; if you don't know the exact path, use the `search_files` tool first.";

    public async Task<string> ExecuteAsync(ToolContext context, IDictionary<string, object?>? arguments)
    {
        var path = arguments.TryGetJsonArg("path");
        try
        {
            // if the file exists and can be read without issue, return the content
            return await fs.ReadFileAsync(context.WorkspaceId, path);
        }
        catch
        {
            // if the file does not exist or cannot be read, search the files in the workspace for similarly named files
            var candidates = await fs.SearchFilesAsync(context.WorkspaceId, Path.GetFileNameWithoutExtension(path));

            // advise the agent on how to recover by bubbling up an error that contains helpful information
            throw new ToolException($"""
                File not found at "{path}".
                
                Similar files:
                {string.Join(Environment.NewLine, candidates.Take(10))}

                If none of these are correct, call search_files with a broader query.
            """);
        }
    }
}