using Model;

namespace Service.Interfaces;
public interface ISetService
{
    Task<ICollection<Set>> GetAllSetsAsync();

    Task<Set> GetSetByIdAsync(string setId);
}
