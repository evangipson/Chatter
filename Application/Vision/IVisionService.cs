namespace Application.Vision;

/// <summary>
/// Responsible for vision functionality, such as image parsing.
/// </summary>
public interface IVisionService
{
    /// <summary>
    /// Analyzes an image buffer, using a prompt for context.
    /// </summary>
    /// <param name="image">The image as a byte buffer.</param>
    /// <param name="prompt">The prompt to use when analyzing the image.</param>
    /// <returns>An awaitable task that contains the analysis results.</returns>
    Task<string> AnalyzeAsync(byte[] image, string prompt);
}
