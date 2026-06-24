using System.Buffers;

namespace Domain.Constants;

public static class ChatConstants
{
    public const string TitlePrompt = """
        Generate a concise conversation title.

        Rules:
        - Maximum 5 words.
        - No quotation marks.
        - No punctuation at the end.
        - Return title only.
    """;

    public static readonly SearchValues<char> PunctuationSearch = SearchValues.Create(".!?,;\"");
}
