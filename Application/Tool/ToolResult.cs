namespace Application.Tool;

/// <summary>
/// Represents the result of getting a tool with <see cref="ToolRegistry"/>.
/// </summary>
/// <param name="success">A flag that, when <see langword="true"/>, denotes <see cref="ITool.ExecuteAsync"/> was successful.</param>
/// <param name="content">The formatted results of <see cref="ITool.ExecuteAsync"/>.</param>
/// <param name="error">An optional error that explains why <see cref="ITool.ExecuteAsync"/> failed.</param>
public readonly struct ToolResult(bool success, string content = "", string? error = null)
{
    /// <summary>
    /// A flag that, when <see langword="true"/>, denotes <see cref="ITool.ExecuteAsync"/> was successful.
    /// </summary>
    public readonly bool Success = success;

    /// <summary>
    /// The formatted results of <see cref="ITool.ExecuteAsync"/>.
    /// </summary>
    public readonly string Content = content;

    /// <summary>
    /// An optional error that explains why <see cref="ITool.ExecuteAsync"/> failed.
    /// </summary>
    public readonly string? Error = error;
}
