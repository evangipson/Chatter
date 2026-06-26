using Microsoft.Extensions.Options;
using Application.Repositories;
using Domain.Models;
using Domain.Options;

namespace Application.Workspaces;

/// <inheritdoc cref="IWorkspaceService"/>
public class WorkspaceService(IWorkspaceRepository workspaceRepository, IOptions<WorkspaceSettings> options) : IWorkspaceService
{
    public async Task<Workspace> CreateAsync(string name)
    {
        var workspaceId = Guid.NewGuid();
        var workspaceRoot = Path.GetFullPath(options.Value.RootDirectory);

        Directory.CreateDirectory(workspaceRoot);
        var workspacePath = Path.Combine(workspaceRoot, workspaceId.ToString());

        Directory.CreateDirectory(workspacePath);
        Workspace workspace = new()
        {
            Id = workspaceId,
            Name = name,
            RootPath = workspacePath,
            CreatedUtc = DateTime.UtcNow,
        };

        await workspaceRepository.AddAsync(workspace);
        await workspaceRepository.SaveChangesAsync();

        return workspace;
    }

    public Task<Workspace?> GetAsync(Guid id) => workspaceRepository.GetByIdAsync(id);

    public Task<List<Workspace>> GetAllAsync() => throw new NotImplementedException();
}