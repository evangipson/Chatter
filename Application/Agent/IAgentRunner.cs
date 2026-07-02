using Domain.Events;
using Domain.Models;

namespace Application.Agent;

/// <summary>
/// Responsible for orchestrating and running the agent feedback loop.
/// </summary>
public interface IAgentRunner
{
    /// <summary>
    /// Orchestrates and runs all tools in the agent feedback loop.
    /// <para>
    /// Broadcasts agent started, tool started, tool finished, and agent finished events.
    /// </para>
    /// </summary>
    /// <param name="chatRequest">The chat request to run the agent for.</param>
    /// <returns>An asynchronous enumerable collection of <see cref="AgentEvent"/>.</returns>
    IAsyncEnumerable<AgentEvent> RunAsync(ChatRequest chatRequest);
}