using Domain.Models;
using Microsoft.Extensions.AI;

namespace Application.Agent;

/// <summary>
/// Responsible for orchestrating and running the agent feedback loop.
/// </summary>
public interface IAgentRunner
{
    /// <summary>
    /// Orchestrates and runs all tools in the agent feedback loop.
    /// </summary>
    /// <param name="context">The current context of the toolchain.</param>
    /// <param name="history">All recent messages in the chat context.</param>
    /// <returns>An awaitable task containing an <see cref="AgentResult"/>.</returns>
    Task<AgentResult> RunAsync(ToolContext context, List<ChatMessage> history);
}