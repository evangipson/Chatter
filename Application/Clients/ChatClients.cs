using Microsoft.Extensions.AI;
using OllamaSharp;
using Domain.Constants;

namespace Application.Clients;

/// <summary>
/// A <see langword="static"/> collection of chat clients.
/// </summary>
public static class ChatClients
{
    private static readonly HttpClient HttpClient = new()
    {
        Timeout = TimeSpan.FromMinutes(20),
        BaseAddress = new Uri(LanguageModelConstants.LanguageModelAddress),
    };

    /// <summary>
    /// A chat client used to stream responses from messages.
    /// </summary>
    public static readonly IChatClient Client = new OllamaApiClient(HttpClient, LanguageModelConstants.UneroModel);
}
