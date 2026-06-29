namespace Domain.Constants;

/// <summary>
/// A <see langword="static"/> collection of conversation values.
/// </summary>
public static class ConversationConstants
{
    /// <summary>
    /// The prompt to instruct the agent to generate a summary for a conversation.
    /// </summary>
    public const string SummaryPrompt = "Summarize the conversation into a compact memory. Keep key facts, decisions, preferences, and goals. Remove fluff. Use bullet points.";

    /// <summary>
    /// The amount of messages required before a conversation title is generated.
    /// </summary>
    public const int TitleWindowSize = 2;

    /// <summary>
    /// The amount of messages required before a conversation summary is generated.
    /// </summary>
    public const int SummaryWindowSize = 50;
}
