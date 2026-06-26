namespace Domain.Models;

/// <summary>
/// Represents the result of searching <paramref name="text"/>.
/// </summary>
/// <param name="file">The file that was searched.</param>
/// <param name="line">The line number <paramref name="text"/> was found.</param>
/// <param name="text">The text that was searched for.</param>
public readonly struct TextSearchResult(string file, int line, string text)
{
    /// <summary>
    /// The file that was searched.
    /// </summary>
    public readonly string File = file;

    /// <summary>
    /// The line number <paramref name="text"/> was found.
    /// </summary>
    public readonly int Line = line;

    /// <summary>
    /// The text that was searched for.
    /// </summary>
    public readonly string Text = text;
}
