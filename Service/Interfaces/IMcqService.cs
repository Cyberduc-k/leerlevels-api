using Model;

namespace Service.Interfaces;

public interface IMcqService
{
    Task<ICollection<Mcq>> GetAllMcqsAsync(int limit, int page);
    Task<ICollection<Mcq>> GetAllMcqsFilteredAsync(int limit, int page, string filter);
    Task<bool> IsBookedmarked(string itemId);

    Task<Mcq> GetMcqByIdAsync(string mcqId);
}
