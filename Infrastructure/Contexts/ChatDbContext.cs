using Microsoft.EntityFrameworkCore;
using Domain.Constants;
using Domain.Models;

namespace Infrastructure.Contexts;

/// <summary>
/// The application database, which contains all persisted conversations, workspaces, and messages.
/// </summary>
/// <param name="options">The options used when setting up the database.</param>
public class ChatDbContext(DbContextOptions<ChatDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // seed the database with the global workspace if there isn't one yet
        optionsBuilder.UseSeeding((context, _) =>
        {
            // try and get the global workspace out of the database, and if it's already there, early exit
            var globalWorkspace = context.Set<Workspace>().FirstOrDefault(x => x.Id.Equals(WorkspaceConstants.GlobalWorkspaceId));
            if (globalWorkspace != null)
            {
                return;
            }

            // set the global workspace in the database, then save the changes
            context.Set<Workspace>().Add(new() {Id = WorkspaceConstants.GlobalWorkspaceId, Name = "Global", RootPath = "Global", CreatedUtc = DateTime.UtcNow});
            context.SaveChanges();
        });
    }

    /// <summary>
    /// All persisted conversations for the application.
    /// </summary>
    public DbSet<ConversationEntity> Conversations => Set<ConversationEntity>();

    /// <summary>
    /// All persisted messages for the application.
    /// </summary>
    public DbSet<ConversationMessageEntity> Messages => Set<ConversationMessageEntity>();

    /// <summary>
    /// All persisted workspaces for the application.
    /// </summary>
    public DbSet<Workspace> Workspaces => Set<Workspace>();
}