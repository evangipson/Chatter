using Application.Repositories;
using Domain.Models;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories;

public class WorkspaceRepository(ChatDbContext db) : IWorkspaceRepository
{
    public async Task<Workspace?> GetByIdAsync(Guid id)
    {
        return await db.Workspaces.FindAsync(id);
    }

    public async Task AddAsync(Workspace workspace)
    {
        await db.Workspaces.AddAsync(workspace);
    }

    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}
