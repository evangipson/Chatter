namespace Application.Tool;

/// <summary>
/// A registry of all tools for the application.
/// </summary>
/// <param name="tools">A collection of all <see cref="ITool"/>.</param>
public sealed class ToolRegistry(IEnumerable<ITool> tools)
{
    private readonly Dictionary<string, ITool> _tools = tools.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// All the tools that are registered for the application.
    /// </summary>
    public IReadOnlyCollection<ITool> Tools => _tools.Values;

    /// <summary>
    /// Gets a tool by <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the tool to get.</param>
    /// <returns>The <see cref="ITool"/> if it was found, defaults to <see langword="null"/>.</returns>
    public ITool? Get(string name) => _tools.TryGetValue(name, out var tool)
        ? tool
        : null;

    /// <summary>
    /// Attempts to find a tool by <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the tool to get.</param>
    /// <param name="tool">The tool that was found.</param>
    /// <returns>A flag that, when <see langword="true"/>, indicates the tool was found.</returns>
    public bool TryGet(string name, out ITool tool) => _tools.TryGetValue(name, out tool!);
}