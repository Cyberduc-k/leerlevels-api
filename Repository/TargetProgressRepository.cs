using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class TargetProgressRepository : Repository<TargetProgress, (string UserId, string TargetId)>, ITargetProgressRepository
{
    public TargetProgressRepository(DataContext context) : base(context, context.TargetProgress)
    {
    }
}
