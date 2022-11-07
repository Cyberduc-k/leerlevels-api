using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class TargetProgressRepository : Repository<TargetProgress, int>, ITargetProgressRepository
{
    public TargetProgressRepository(DataContext context) : base(context, context.TargetProgress)
    {
    }
}
