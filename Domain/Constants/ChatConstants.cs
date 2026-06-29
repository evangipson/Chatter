using System.Buffers;

namespace Domain.Constants;

/// <summary>
/// A <see langword="static"/> collection of chat values.
/// </summary>
public static class ChatConstants
{
    /// <summary>
    /// The prompt used to instruct the agent to generate a title for a chat.
    /// </summary>
    public const string TitlePrompt = """
        Generate a concise conversation title.

        Rules:
        - Maximum 5 words.
        - No quotation marks.
        - No punctuation at the end.
        - Return title only.
    """;

    /// <summary>
    /// The result of a punctuation search, intended for use to sanitize text to speech output.
    /// </summary>
    public static readonly SearchValues<char> PunctuationSearch = SearchValues.Create(".!?,;\"");
}
