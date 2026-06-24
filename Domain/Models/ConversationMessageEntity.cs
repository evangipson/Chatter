namespace Domain.Models;

public class ConversationMessageEntity
{
    public Guid Id { get; set; }

    public Guid ConversationId { get; set; }

    public string Role { get; set; } = "";

    public string Content { get; set; } = "";

    public DateTime CreatedUtc { get; set; }
}
