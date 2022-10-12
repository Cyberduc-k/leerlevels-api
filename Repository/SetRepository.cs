using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class SetRepository : Repository<Set>, ISetRepository
{
    public SetRepository(DataContext context) : base(context, context.Sets)
    {
    }
}
