using Data;
using Model;
using Repository.Interfaces;

namespace Repository;

public class ForumReplyRepository : Repository<ForumReply>, IForumReplyRepository
{
    public ForumReplyRepository(DataContext context) : base(context, context.Replies)
    {
    }
}
