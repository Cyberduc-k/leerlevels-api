using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class TargetRepository : Repository<Target, string>, ITargetRepository
{
    public TargetRepository(DataContext context) : base(context, context.Targets)
    {
    }
}
