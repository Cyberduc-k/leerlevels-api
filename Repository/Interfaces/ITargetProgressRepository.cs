using Model;

namespace Repository.Interfaces;

public interface ITargetProgressRepository : IRepository<TargetProgress, (string UserId, string TargetId)>
{
}
