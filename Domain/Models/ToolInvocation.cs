namespace Domain.Models;

/// <summary>
/// Represents the result of calling a tool, usually from an agent.
/// </summary>
/// <param name="toolName">The name of the tool that was called.</param>
/// <param name="duration">The total time elapsed during the tool call.</param>
/// <param name="result">The human-readable result of the tool call.</param>
/// <param name="arguments">The optional arguments provided to the tool call.</param>
/// <param name="success">A flag that, when <see langword="true"/>, denotes a successful tool call.</param>
public readonly struct ToolInvocation(string toolName, TimeSpan duration, string? result, string? arguments = null, bool success = false)
{
    /// <summary>
    /// The name of the tool that was called.
    /// </summary>
    public string ToolName => toolName;

    /// <summary>
    /// The total time elapsed during the tool call.
    /// </summary>
    public TimeSpan Duration => duration;

    /// <summary>
    /// The human-readable result of the tool call.
    /// </summary>
    public string? Result => result;

    /// <summary>
    /// The optional arguments provided to the tool call.
    /// </summary>
    public string? Arguments => arguments;

    /// <summary>
    /// A flag that, when <see langword="true"/>, denotes a successful tool call.
    /// </summary>
    public bool Success => success;
}