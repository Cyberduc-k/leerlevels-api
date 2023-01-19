using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface ISetService
{
    Task<ICollection<Set>> GetAllSetsAsync(int limit, int page);
    Task<ICollection<Set>> GetAllSetsFilteredAsync(int limit, int page, string filter);

    Task<Set> GetSetByIdAsync(string setId);

    public Task<Set> CreateSet(Set newSet);
    public Task UpdateSet(string setId, UpdateSetDTO changes);
    public Task DeleteSet(string setId);
}
