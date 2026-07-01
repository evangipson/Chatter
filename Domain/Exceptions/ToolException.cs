namespace Domain.Exceptions;

/// <summary>
/// An <see cref="Exception"/> that indicates a runtime error with a Chatter tool.
/// </summary>
/// <param name="message">The recovery message to send to the agent.</param>
public sealed class ToolException(string message) : Exception(message);