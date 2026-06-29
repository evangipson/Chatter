using Domain.Constants;
using OllamaSharp;

namespace Application.Clients;

/// <summary>
/// A <see langword="static"/> collection of all HTTP clients.
/// </summary>
public static class HttpClients
{
    private static readonly Uri _baseAddress = new(LanguageModelConstants.LanguageModelAddress);
    private static readonly HttpClient _httpClient = new() { BaseAddress = _baseAddress, Timeout = TimeSpan.FromMinutes(5) };

    /// <summary>
    /// The application's API client.
    /// <para>
    /// Responsible for local communication with Ollama.
    /// </para>
    /// </summary>
    public static readonly OllamaApiClient ApiClient = new(_httpClient, LanguageModelConstants.Qwen36Model);
}
