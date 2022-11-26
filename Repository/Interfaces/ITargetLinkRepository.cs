using Model;

namespace Repository.Interfaces;

public interface ITargetLinkRepository : IRepository<TargetLink, (string FromId, string ToId)>
{
}
