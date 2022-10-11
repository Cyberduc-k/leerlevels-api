using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface IForumService
{
    public Task<ICollection<Forum>> GetAll();
    public Task<Forum> CreateForum(Forum newForum);
    public Task<Forum> GetById(string forumId);
    public Task UpdateForum(string forumId, UpdateForumDTO changes);
    public Task<ForumReply> AddReply(string forumId, ForumReply reply);
    public Task UpdateForumReply(string replyId, UpdateForumReplyDTO changes);
    public Task DeleteForumReply(string forumId, string replyId);
}
