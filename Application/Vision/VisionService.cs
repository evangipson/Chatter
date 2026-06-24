namespace Application.Vision;

public class VisionService : IVisionService
{
    public Task<string> AnalyzeAsync(byte[] image, string prompt)
    {
        return Task.FromResult(string.Empty);
    }
}
