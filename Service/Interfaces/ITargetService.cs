using Model;

namespace Service.Interfaces;
public interface ITargetService
{
    Task<ICollection<Target>> GetAllTargetsAsync(int limit, int page);
    Task<ICollection<Target>> GetAllTargetsFilteredAsync(int limit, int page, string filter);

    Task<Target> GetTargetByIdAsync(string targetId);
}
