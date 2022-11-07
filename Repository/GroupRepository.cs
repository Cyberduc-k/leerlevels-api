using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class GroupRepository : Repository<Group, string>, IGroupRepository
{
    public GroupRepository(DataContext context) : base(context, context.Groups)
    {
    }
}
