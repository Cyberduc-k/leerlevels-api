using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface ITargetService
{
    Task<ICollection<Target>> GetAllTargetsAsync(int limit, int page);
    Task<ICollection<Target>> GetAllTargetsFilteredAsync(int limit, int page, string filter);

    Task<Target> GetTargetByIdAsync(string targetId);
    Task<int> GetTargetCountAsync();

    public Task<Target> CreateTarget(Target newTarget);
    public Task UpdateTarget(string targetId, UpdateTargetDTO changes);
    public Task DeleteTarget(string targetId);
}
