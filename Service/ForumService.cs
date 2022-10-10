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
}
