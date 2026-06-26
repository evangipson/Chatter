using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Application.Repositories;
using Domain.Models;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories;

/// <inheritdoc cref="IConversationRepository"/>
public class ConversationRepository(ChatDbContext db) : IConversationRepository
{
    public async Task<List<ConversationEntity>> GetByWorkspaceId(Guid workspaceId) => await db.Conversations
        .Where(x => x.WorkspaceId.Equals(workspaceId))
        .OrderByDescending(x => x.LastUpdatedUtc)
        .ToListAsync();

    public async Task<ConversationEntity?> GetAsync(Guid id) => await db.Conversations
        .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ConversationEntity> CreateAsync(Guid workspaceId)
    {
        ConversationEntity conversation = new()
        {
            Id = Guid.NewGuid(),
            WorkspaceId = workspaceId,
            Title = "New Chat",
            CreatedUtc = DateTime.UtcNow,
            LastUpdatedUtc = DateTime.UtcNow
        };

        db.Conversations.Add(conversation);
        await db.SaveChangesAsync();

        return conversation;
    }

    public async Task DeleteAsync(Guid id)
    {
        // get the conversation, and if one isn't found, early exit
        var conversation = await db.Conversations.FindAsync(id);
        if (conversation == null)
        {
            return;
        }

        // delete the conversation and all conversation messages
        db.Conversations.Remove(conversation);
        var messages = db.Messages.Where(m => m.ConversationId == id);
        db.Messages.RemoveRange(messages);

        // save the deletion
        await db.SaveChangesAsync();
    }

    public async Task AddMessageAsync(Guid conversationId, ChatMessage message)
    {
        db.Messages.Add(new ConversationMessageEntity
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            Role = message.Role.Value,
            Content = message.Text,
            CreatedUtc = DateTime.UtcNow
        });

        await db.SaveChangesAsync();
    }

    public async Task<List<ChatMessage>> GetMessagesAsync(Guid conversationId)
    {
        var messages = await db.Messages
            .Where(x => x.ConversationId == conversationId)
            .OrderBy(x => x.CreatedUtc)
            .ToListAsync();

        return [.. messages.Select(x => new ChatMessage(new ChatRole(x.Role), x.Content))];
    }

    public async Task UpdateTitleAsync(Guid conversationId, string title)
    {
        await db.Conversations
            .Where(x => x.Id == conversationId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Title, title));
    }

    public async Task UpdateSummaryAsync(Guid conversationId, string summary)
    {
        await db.Conversations
            .Where(x => x.Id == conversationId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Summary, summary));
    }

    public async Task TouchAsync(Guid conversationId)
    {
        await db.Conversations
            .Where(x => x.Id == conversationId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.LastUpdatedUtc, DateTime.UtcNow));
    }
}
