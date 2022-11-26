using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class TargetLinkRepository : Repository<TargetLink, (string FromId, string ToId)>, ITargetLinkRepository
{
    public TargetLinkRepository(DataContext context) : base(context, context.Links)
    {
    }
}
