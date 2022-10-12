using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class ForumRepository : Repository<Forum>, IForumRepository
{
    public ForumRepository(DataContext context) : base(context, context.Forums)
    {
    }
}
