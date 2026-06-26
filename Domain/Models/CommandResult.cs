namespace Domain.Models;

/// <summary>
/// Represents the results of a command run by a tool.
/// </summary>
/// <param name="exitCode">The exit code from a tool command.</param>
/// <param name="stdOut">The standard output from a tool command.</param>
/// <param name="stdErr">The standard error from a tool command.</param>
public readonly struct CommandResult(int exitCode, string stdOut, string stdErr)
{
    /// <summary>
    /// The exit code from a tool command.
    /// </summary>
    public readonly int ExitCode = exitCode;

    /// <summary>
    /// The standard output from a tool command.
    /// </summary>
    public readonly string StdOut = stdOut;

    /// <summary>
    /// The standard error from a tool command.
    /// </summary>
    public readonly string StdErr = stdErr;
}
