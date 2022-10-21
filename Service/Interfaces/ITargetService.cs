using Model;

namespace Service.Interfaces;
public interface ITargetService
{
    Task<ICollection<Target>> GetAllTargetsAsync();

    Task<Target> GetTargetByIdAsync(string targetId);
}
