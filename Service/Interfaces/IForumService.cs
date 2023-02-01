using Model;
using Model.DTO;

namespace Service.Interfaces;

public interface IForumService
{
    /**
     * <summary>
     * Get all forum posts.
     * </summary>
     */
    public Task<ICollection<Forum>> GetAll();

    /**
     * <summary>
     * Create a new forum post.
     * </summary>
     */
    public Task<Forum> CreateForum(Forum newForum);

    /**
     * <summary>
     * Get a single forum post by its id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task<Forum> GetById(string forumId);

    /**
     * <summary>
     * Get a single reply by its id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task<ForumReply> GetReplyById(string replyId);

    /**
     * <summary>
     * Update the <see cref="Forum.Title"/> or <see cref="Forum.Description"/> of the forum post with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task UpdateForum(string forumId, UpdateForumDTO changes);

    /**
     * <summary>
     * Delete the forum post with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task DeleteForum(string forumId);

    /**
     * <summary>
     * Add a new reply to the forum post with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task<ForumReply> AddReply(string forumId, ForumReply reply);

    /**
     * <summary>
     * Change the <see cref="ForumReply.Text"/> or <see cref="ForumReply.Upvotes"/> of the reply with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task UpdateForumReply(string replyId, UpdateForumReplyDTO changes);

    /**
     * <summary>
     * Delete the reply with the given id.
     * </summary>
     * <exception cref="Exceptions.NotFoundException"/>
     */
    public Task DeleteForumReply(string forumId, string replyId);
}
