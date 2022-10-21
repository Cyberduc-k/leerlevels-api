using Model;

namespace Service.Interfaces;
public interface IMcqService
{
    Task<ICollection<Mcq>> GetAllMcqsAsync();

    Task<Mcq> GetMcqByIdAsync(string mcqId);
}
