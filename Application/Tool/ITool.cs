using Domain.Models;

namespace Application.Tool;

/// <summary>
/// Represents a set of work that is run by an agent.
/// </summary>
public interface ITool
{
    /// <summary>
    /// The name of the tool.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The description of the tool.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Invokes the set of work.
    /// </summary>
    /// <param name="context">The current tool context.</param>
    /// <param name="arguments">Any arguments that are needed for the set of work.</param>
    /// <returns>An awaitable task that contains the work results.</returns>
    Task<string> ExecuteAsync(ToolContext context, IDictionary<string, object?>? arguments);
}