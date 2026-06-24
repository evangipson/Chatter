using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Infrastructure.Contexts;

public class ChatDbContext(DbContextOptions<ChatDbContext> options) : DbContext(options)
{
    public DbSet<ConversationEntity> Conversations => Set<ConversationEntity>();

    public DbSet<ConversationMessageEntity> Messages => Set<ConversationMessageEntity>();

    public DbSet<Workspace> Workspaces => Set<Workspace>();
}