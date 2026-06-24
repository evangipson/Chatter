using Domain.Models;

namespace Application.Workspaces;

public interface IWorkspaceService
{
    Task<Workspace> CreateAsync(string name);

    Task<Workspace?> GetAsync(Guid id);

    Task<List<Workspace>> GetAllAsync();
}
