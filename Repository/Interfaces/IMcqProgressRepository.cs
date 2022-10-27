using Model;

namespace Repository.Interfaces;

public interface IMcqProgressRepository : IRepository<McqProgress, (string UserId, string McqId)>
{
}
