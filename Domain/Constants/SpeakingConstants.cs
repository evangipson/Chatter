namespace Domain.Constants;

/// <summary>
/// A <see langword="static"/> collection of text-to-speech values.
/// </summary>
public static class SpeakingConstants
{
    /// <summary>
    /// The key for the directory in which chat models are stored.
    /// </summary>
    public const string ChatModelsKey = "ChatModels";

    /// <summary>
    /// The file name of the text-to-speech model.
    /// </summary>
    public const string ModelFileName = "en_US-libritts_r-medium.onnx";

    /// <summary>
    /// The file path of the tokens for the text-to-speech model.
    /// </summary>
    public const string TokensFileName = "tokens.txt";

    /// <summary>
    /// The <b>absolute</b> directory where text-to-speech data is stored.
    /// </summary>
    public const string TextToSpeechDataDirectory = @"C:\Program Files\piper\espeak-ng-data";

    /// <summary>
    /// The default wave rate of generated text-to-speech.
    /// </summary>
    public const int WaveRate = 22050;

    /// <summary>
    /// The default bitrate of generated text-to-speech.
    /// </summary>
    public const int WaveBits = 16;

    /// <summary>
    /// The default number of channels for generated text-to-speech.
    /// </summary>
    public const int WaveChannels = 1;
}
