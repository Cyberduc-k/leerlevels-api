using Model;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service;

public class ForumService : IForumService
{
    private readonly IForumRepository _forumRepository;

    public ForumService(IForumRepository forumRepository)
    {
        _forumRepository = forumRepository;
    }

    public async Task<ICollection<Forum>> GetAllAsync()
    {
        return await _forumRepository.GetAllAsync().ToArrayAsync();
    }

    public async Task<Forum> CreateForum(Forum newForum)
    {
        newForum.Id = Guid.NewGuid().ToString();
        await _forumRepository.InsertAsync(newForum);
        await _forumRepository.SaveChanges();
        return newForum;
    }
}
