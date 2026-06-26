namespace Domain.Models;

public class ConversationEntity
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }

    public Workspace Workspace { get; set; } = null!;

    public string Title { get; set; } = "New Chat";

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime LastUpdatedUtc { get; set; } = DateTime.UtcNow;

    public string? Summary { get; set; }

    public int MessageCount { get; set; }
}
