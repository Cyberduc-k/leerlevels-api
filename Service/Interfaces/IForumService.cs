using Model;

namespace Service.Interfaces;

public interface IForumService
{
    public Task<ICollection<Forum>> GetAllAsync();
    public Task<Forum> CreateForum(Forum newForum);
}
