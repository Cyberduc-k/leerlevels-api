using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class SetRepository : Repository<Set, string>, ISetRepository
{
    public SetRepository(DataContext context) : base(context, context.Sets)
    {
    }
}
