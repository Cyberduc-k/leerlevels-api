using Model;

namespace Service.Interfaces;

public interface ISetService
{
    Task<ICollection<Set>> GetAllSetsAsync(int limit, int page);
    Task<ICollection<Set>> GetAllSetsFilteredAsync(int limit, int page, string filter);

    Task<Set> GetSetByIdAsync(string setId);
}
