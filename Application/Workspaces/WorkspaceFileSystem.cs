using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Application.Repositories;
using Domain.Models;

namespace Application.Workspaces;

/// <inheritdoc cref="IWorkspaceFileSystem"/>
public class WorkspaceFileSystem(IWorkspaceRepository repo, ILogger<WorkspaceFileSystem> logger) : IWorkspaceFileSystem
{
    public async Task<List<string>> GetFilesAsync(Guid workspaceId)
    {
        logger.LogInformation("{LogPrefix} getting files for workspace \"{WorkspaceId}\"...", $"[{nameof(WorkspaceFileSystem)}.{nameof(GetFilesAsync)}]:", workspaceId);
        var workspace = await GetWorkspaceAsync(workspaceId);
        List<string> files = [..Directory
            .EnumerateFiles(workspace.RootPath, "*", SearchOption.AllDirectories)
            .Select(path => Path.GetRelativePath(workspace.RootPath, path))];
        logger.LogInformation("{LogPrefix} got {FileCount} files for workspace \"{WorkspaceId}\".", $"[{nameof(WorkspaceFileSystem)}.{nameof(GetFilesAsync)}]:", files.Count, workspaceId);
        return files;
    }

    public async Task<string> ReadFileAsync(Guid workspaceId, string relativePath)
    {
        logger.LogInformation("{LogPrefix} reading file \"{FilePath}\" for workspace \"{WorkspaceId}\"...", $"[{nameof(WorkspaceFileSystem)}.{nameof(ReadFileAsync)}]:", relativePath, workspaceId);
        var workspace = await GetWorkspaceAsync(workspaceId);
        var fullPath = ResolvePath(workspace, relativePath);
        var readContent = await File.ReadAllTextAsync(fullPath);
        logger.LogInformation("{LogPrefix} read all {FileContentLength} bytes from file \"{FilePath}\" for workspace \"{WorkspaceId}\".", $"[{nameof(WorkspaceFileSystem)}.{nameof(ReadFileAsync)}]:", readContent.Length, relativePath, workspaceId);
        return readContent;
    }

    public async Task<List<string>> SearchFilesAsync(Guid workspaceId, string pattern)
    {
        logger.LogInformation("{LogPrefix} searching files for the pattern \"{Pattern}\" in workspace \"{WorkspaceId}\"...", $"[{nameof(WorkspaceFileSystem)}.{nameof(SearchFilesAsync)}]:", pattern, workspaceId);
        var files = await GetFilesAsync(workspaceId);
        List<string> found = [.. files.Where(file => file.Contains(pattern, StringComparison.OrdinalIgnoreCase))];
        logger.LogInformation("{LogPrefix} found {FoundCount} files for the pattern \"{Pattern}\" in workspace \"{WorkspaceId}\".", $"[{nameof(WorkspaceFileSystem)}.{nameof(SearchFilesAsync)}]:", found.Count, pattern, workspaceId);
        return found;
    }

    public async Task<List<TextSearchResult>> SearchTextAsync(Guid workspaceId, string text, int maxResults = 300)
    {
        logger.LogInformation("{LogPrefix} searching text for the pattern \"{Pattern}\" in workspace \"{WorkspaceId}\"...", $"[{nameof(WorkspaceFileSystem)}.{nameof(SearchFilesAsync)}]:", text, workspaceId);
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

        logger.LogInformation("{LogPrefix} found {ResultCount} results for the pattern \"{Pattern}\" in workspace \"{WorkspaceId}\"...", $"[{nameof(WorkspaceFileSystem)}.{nameof(SearchFilesAsync)}]:", results.Count, text, workspaceId);
        return results;
    }

    public async Task WriteFileAsync(Guid workspaceId, string relativePath, string contents)
    {
        logger.LogInformation("{LogPrefix} writing file \"{FileName}\" in workspace \"{WorkspaceId}\"...", $"[{nameof(WorkspaceFileSystem)}.{nameof(WriteFileAsync)}]:", relativePath, workspaceId);
        Workspace workspace = await GetWorkspaceAsync(workspaceId);
        string fullPath = ResolvePath(workspace, relativePath);
        await File.WriteAllTextAsync(fullPath, contents);
        logger.LogInformation("{LogPrefix} finished writing {ContentLength} bytes to file \"{FileName}\" in workspace \"{WorkspaceId}\".", $"[{nameof(WorkspaceFileSystem)}.{nameof(WriteFileAsync)}]:", contents.Length, relativePath, workspaceId);
    }

    public async Task<CommandResult> RunCommandAsync(Guid workspaceId, string command, string arguments)
    {
        logger.LogInformation("{LogPrefix} running command \"{CommandName}\" in workspace \"{WorkspaceId}\"...", $"[{nameof(WorkspaceFileSystem)}.{nameof(RunCommandAsync)}]:", command, workspaceId);
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
        logger.LogInformation("{LogPrefix} done running command \"{CommandName}\" in workspace \"{WorkspaceId}\".\n\tstandard out: {StdOut}\n\tstandard error: {StdErr}", $"[{nameof(WorkspaceFileSystem)}.{nameof(RunCommandAsync)}]:", command, workspaceId, stdout, stderr);
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