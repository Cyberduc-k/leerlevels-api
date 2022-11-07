using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class ForumRepository : Repository<Forum, string>, IForumRepository
{
    public ForumRepository(DataContext context) : base(context, context.Forums)
    {
    }
}
