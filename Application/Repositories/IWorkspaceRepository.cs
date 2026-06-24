using Domain.Models;

namespace Application.Repositories;

public interface IWorkspaceRepository
{
    Task<Workspace?> GetByIdAsync(Guid id);

    Task AddAsync(Workspace workspace);

    Task SaveChangesAsync();
}
