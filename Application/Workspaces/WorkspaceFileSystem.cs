using System.Diagnostics;
using Application.Repositories;
using Domain.Models;

namespace Application.Workspaces;

/// <inheritdoc cref="IWorkspaceFileSystem"/>
public class WorkspaceFileSystem(IWorkspaceRepository repo) : IWorkspaceFileSystem
{
    public async Task<List<string>> GetFilesAsync(Guid workspaceId)
    {
        var workspace = await GetWorkspaceAsync(workspaceId);
        return [..Directory
            .EnumerateFiles(workspace.RootPath, "*", SearchOption.AllDirectories)
            .Select(path => Path.GetRelativePath(workspace.RootPath, path))];
    }

    public async Task<string> ReadFileAsync(Guid workspaceId, string relativePath)
    {
        var workspace = await GetWorkspaceAsync(workspaceId);
        var fullPath = ResolvePath(workspace, relativePath);
        return await File.ReadAllTextAsync(fullPath);
    }

    public async Task<List<string>> SearchFilesAsync(Guid workspaceId, string pattern)
    {
        var files = await GetFilesAsync(workspaceId);
        return [.. files.Where(file => file.Contains(pattern, StringComparison.OrdinalIgnoreCase))];
    }

    public async Task<List<TextSearchResult>> SearchTextAsync(Guid workspaceId, string text, int maxResults = 300)
    {
        var workspace = await GetWorkspaceAsync(workspaceId);

        List<TextSearchResult> results = [];
        foreach (string relativePath in await GetFilesAsync(workspaceId))
        {
            var fullPath = ResolvePath(workspace, relativePath);
            var lines = await File.ReadAllLinesAsync(fullPath);

            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].Contains(text, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                results.Add(new(file: relativePath, line: i + 1, text: GetSnippet(lines, i)));
                if (results.Count >= maxResults)
                {
                    return results;
                }
            }
        }

        return results;
    }

    public async Task WriteFileAsync(Guid workspaceId, string relativePath, string contents)
    {
        Workspace workspace = await GetWorkspaceAsync(workspaceId);
        string fullPath = ResolvePath(workspace, relativePath);
        await File.WriteAllTextAsync(fullPath, contents);
    }

    public async Task<CommandResult> RunCommandAsync(Guid workspaceId, string command, string arguments)
    {
        Workspace workspace = await GetWorkspaceAsync(workspaceId);

        var process = Process.Start(new ProcessStartInfo()
        {
            FileName = command,
            Arguments = arguments,
            WorkingDirectory = workspace.RootPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        })!;

        var stdout = await process.StandardOutput.ReadToEndAsync();
        var stderr = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        return new CommandResult(exitCode: process.ExitCode, stdOut: stdout, stdErr: stderr);
    }

    private async Task<Workspace> GetWorkspaceAsync(Guid workspaceId) => (await repo.GetByIdAsync(workspaceId))
        ?? throw new InvalidOperationException($"Workspace \"{workspaceId}\" was not found.");

    private static string ResolvePath(Workspace workspace, string relativePath)
    {
        var fullPath = Path.GetFullPath(Path.Combine(workspace.RootPath, relativePath));
        return fullPath.StartsWith(workspace.RootPath, StringComparison.OrdinalIgnoreCase)
            ? fullPath
            : throw new InvalidOperationException("Attempted to access a file outside the workspace.");
    }

    private static string GetSnippet(string[] lines, int lineIndex)
    {
        var start = Math.Max(0, lineIndex - 2);
        var end = Math.Min(lines.Length - 1, lineIndex + 2);
        return string.Join(Environment.NewLine, lines.Skip(start).Take(end - start + 1));
    }
}