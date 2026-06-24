using Domain.Constants;
using SherpaOnnx;

namespace Application.Clients;

/// <summary>
/// A <see langword="static"/> collection of speaking clients.
/// </summary>
public static class SpeakingClients
{
    /// <summary>
    /// A speaking client used to stream text-to-speech audio.
    /// </summary>
    public static readonly OfflineTts Client = new(_config);

    private static readonly OfflineTtsConfig _config = new() { Model = _configModel };

    private static readonly OfflineTtsModelConfig _configModel = new() { Vits = _configModelVits };

    private static readonly OfflineTtsVitsModelConfig _configModelVits = new()
    {
        Model = Path.Combine(AppContext.BaseDirectory, SpeakingConstants.ChatModelsKey, SpeakingConstants.ModelFileName),
        Tokens = Path.Combine(AppContext.BaseDirectory, SpeakingConstants.ChatModelsKey, SpeakingConstants.TokensFileName),
        DataDir = SpeakingConstants.TextToSpeechDataDirectory
    };
}
