using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface IMcqService
{
    Task<ICollection<Mcq>> GetAllMcqsAsync(int limit, int page);
    Task<ICollection<Mcq>> GetAllMcqsFilteredAsync(int limit, int page, string filter);
    Task<Mcq> GetMcqByIdAsync(string mcqId);

    public Task<Mcq> CreateMcq(Mcq newMcq);
    public Task UpdateMcq(string mcqId, UpdateMcqDTO changes);
    public Task DeleteMcq(string mcqId);
}
