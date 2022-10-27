using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class McqProgressRepository : Repository<McqProgress, int>, IMcqProgressRepository
{
    public McqProgressRepository(DataContext context) : base(context, context.McqProgress)
    {
    }
}
