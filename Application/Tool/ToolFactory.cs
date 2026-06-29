using Domain.Models;
using Microsoft.Extensions.AI;

namespace Application.Tool;

/// <summary>
/// Responsible for creating <see cref="AIFunction"/> representations from <see cref="ITool"/>.
/// </summary>
public sealed class ToolFactory
{
    /// <summary>
    /// Creates a collection of <see cref="AIFunction"/> from a collection of <see cref="ITool"/>.
    /// </summary>
    /// <param name="tools">The tools to create <see cref="AIFunction"/> representations of.</param>
    /// <param name="context">The current tool context.</param>
    /// <returns>A <see langword="new"/> <see langword="readonly"/> collection of <see cref="AIFunction"/>.</returns>
    public static IReadOnlyList<AIFunction> Create(IEnumerable<ITool> tools, ToolContext context)
        => [.. tools.Select(x => CreateFunction(x, context))];

    private static AIFunction CreateFunction(ITool tool, ToolContext context) => AIFunctionFactory
        .Create((IDictionary<string, object?> args) => tool.ExecuteAsync(context, args), name: tool.Name, description: tool.Description);
}
