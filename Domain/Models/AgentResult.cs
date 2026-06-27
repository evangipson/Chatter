namespace Domain.Models;

/// <summary>
/// Represents the results of an agent run.
/// </summary>
/// <param name="response">The human-readable response of an agent run.</param>
/// <param name="duration">The total time elapsed during an agent run.</param>
/// <param name="toolCallResults">An optional collection of tool call results from an agent run.</param>
public readonly struct AgentResult(string response, TimeSpan duration, IEnumerable<ToolInvocation>? toolCallResults = null)
{
    /// <summary>
    /// The human-readable response of an agent run.
    /// </summary>
    public string Response => response;

    /// <summary>
    /// The total time elapsed during an agent run.
    /// </summary>
    public TimeSpan Duration => duration;

    /// <summary>
    /// The collection of tool call results from an agent run.
    /// <para>
    /// Defaults to an empty collection, will never be <see langword="null"/>.
    /// </para>
    /// </summary>
    public List<ToolInvocation> ToolInvocations => [.. toolCallResults ?? []];
}