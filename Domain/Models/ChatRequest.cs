namespace Domain.Models;

public class ChatRequest
{
    public Guid ConversationId { get; set; }

    public Guid? WorkspaceId { get; set; }

    public string? Message { get; set; }

    public string? ImageBase64 { get; init; }

    public string? SystemPrompt { get; set; }
}