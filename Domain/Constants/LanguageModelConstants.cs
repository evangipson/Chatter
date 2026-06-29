namespace Domain.Constants;

/// <summary>
/// A <see langword="static"/> collection of language model values.
/// </summary>
public static class LanguageModelConstants
{
    /// <summary>
    /// The local address (URI) for the language model.
    /// </summary>
    public const string LanguageModelAddress = "http://localhost:11434";

    /// <summary>
    /// The name of the SFT language model.
    /// </summary>
    public const string SftModel = "sft";

    /// <summary>
    /// The name of the Unero language model.
    /// </summary>
    public const string UneroModel = "unero";

    /// <summary>
    /// The name of the B language model.
    /// </summary>
    public const string BModel = "3b";

    /// <summary>
    /// The name of the Qwen3.6:35b language model.
    /// </summary>
    public const string Qwen36_35bModel = "qwen3.6:35b";

    /// <summary>
    /// The name of the standard Qwen3.6 language model.
    /// </summary>
    public const string Qwen36Model = "qwen3.6:latest";

    /// <summary>
    /// The name of the Gemma-4 "Fable" language model.
    /// </summary>
    public const string FableModel = "hf.co/yuxinlu1/gemma-4-12B-coder-fable5-composer2.5-v1-GGUF:Q4_K_M";
}
