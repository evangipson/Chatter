namespace Domain.Models;

/// <summary>
/// Represents all necessary information to create a file.
/// </summary>
/// <param name="Path">The path of the new file.</param>
/// <param name="Content">The content of the new file.</param>
public record CreateFileRequest(string Path, string Content);