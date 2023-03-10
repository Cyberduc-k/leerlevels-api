using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;

namespace Service;

public class ForumService : IForumService
{
    private readonly ILogger _logger;
    private readonly IForumRepository _forumRepository;
    private readonly IForumReplyRepository _forumReplyRepository;

    public ForumService(ILoggerFactory loggerFactory, IForumRepository forumRepository, IForumReplyRepository forumReplyRepository)
    {
        _logger = loggerFactory.CreateLogger<ForumService>();
        _forumRepository = forumRepository;
        _forumReplyRepository = forumReplyRepository;
    }

    public async Task<ICollection<Forum>> GetAll()
    {
        return await _forumRepository.Include(f => f.Replies).GetAllAsync().ToArrayAsync();
    }

    public async Task<Forum> CreateForum(Forum newForum)
    {
        newForum.Id = Guid.NewGuid().ToString();
        newForum.Replies = new List<ForumReply>();

        await _forumRepository.InsertAsync(newForum);
        await _forumRepository.SaveChanges();
        return newForum;
    }

    public async Task<Forum> GetById(string forumId)
    {
        return await _forumRepository.Include(f => f.Replies).GetByAsync(f => f.Id == forumId) ?? throw new NotFoundException("forum post");
    }

    public async Task<ForumReply> GetReplyById(string replyId)
    {
        return await _forumReplyRepository.GetByIdAsync(replyId) ?? throw new NotFoundException("forum post reply");
    }

    public async Task UpdateForum(string forumId, UpdateForumDTO changes)
    {
        Forum forum = await GetById(forumId);
        forum.Title = changes.Title ?? forum.Title;
        forum.Description = changes.Description ?? forum.Description;
        await _forumRepository.SaveChanges();
    }

    public async Task DeleteForum(string forumId)
    {
        Forum forum = await GetById(forumId);
        _forumRepository.Remove(forum);
        await _forumRepository.SaveChanges();
    }

    public async Task<ForumReply> AddReply(string forumId, ForumReply reply)
    {
        Forum forum = await GetById(forumId);

        reply.Id = Guid.NewGuid().ToString();
        forum.Replies.Add(reply);

        await _forumReplyRepository.InsertAsync(reply);

        // Only need to call SaveChanges on one of the repositories because both share the same ForumContext.
        await _forumRepository.SaveChanges();
        return reply;
    }

    public async Task UpdateForumReply(string replyId, UpdateForumReplyDTO changes)
    {
        ForumReply reply = await GetReplyById(replyId);
        reply.Text = changes.Text ?? reply.Text;
        reply.Upvotes = changes.Upvotes ?? reply.Upvotes;
        await _forumReplyRepository.SaveChanges();
    }

    public async Task DeleteForumReply(string forumId, string replyId)
    {
        Forum forum = await GetById(forumId);
        ForumReply reply = await GetReplyById(replyId);

        forum.Replies.Remove(reply);
        _forumReplyRepository.Remove(reply);
        await _forumRepository.SaveChanges();
    }
}
