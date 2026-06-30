namespace Domain.Constants;

/// <summary>
/// A <see langword="static"/> collection of constant event values.
/// </summary>
public static class EventConstants
{
    /// <summary>
    /// The key for the event when an agent is finished.
    /// </summary>
    public const string AgentFinished = "agent_finished";

    /// <summary>
    /// The key for the event when an agent is started.
    /// </summary>
    public const string AgentStarted = "agent_started";

    /// <summary>
    /// The key for the event when an assistant is finished broadcasting tokens.
    /// </summary>
    public const string AssistantFinished = "assistant_finished";

    /// <summary>
    /// The key for the event when an assistant starts broadcasting tokens.
    /// </summary>
    public const string AssistantStarted = "assistant_started";

    /// <summary>
    /// The key for the event when an assistant broadcasts a token.
    /// </summary>
    public const string AssistantToken = "assistant_token";

    /// <summary>
    /// The key for the event when an agent has finished running a tool.
    /// </summary>
    public const string ToolFinished = "tool_finished";

    /// <summary>
    /// The key for the event when an agent has started running a tool.
    /// </summary>
    public const string ToolStarted = "tool_started";

    /// <summary>
    /// The key for the event when an agent has finished creating a workspace.
    /// </summary>
    public const string WorkspaceCreated = "workspace_created";
}