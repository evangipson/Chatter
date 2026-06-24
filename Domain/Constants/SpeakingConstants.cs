namespace Domain.Constants;

public static class SpeakingConstants
{
    public const string ChatModelsKey = "ChatModels";

    public const string ModelFileName = "en_US-libritts_r-medium.onnx";

    public const string TokensFileName = "tokens.txt";

    public const string TextToSpeechDataDirectory = @"C:\Program Files\piper\espeak-ng-data";

    public const int WaveRate = 22050;

    public const int WaveBits = 16;

    public const int WaveChannels = 1;
}
