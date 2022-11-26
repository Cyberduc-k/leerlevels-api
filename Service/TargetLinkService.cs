using Repository.Interfaces;
using Service.Interfaces;

namespace Service;

public class TargetLinkService : ITargetLinkService
{
    private readonly ITargetLinkRepository _repository;

    public TargetLinkService(ITargetLinkRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<string>> GetNextTargets(string targetId)
    {
        return await _repository
            .Where(l => l.FromId == targetId)
            .Select(l => l.ToId)
            .GetAllAsync()
            .ToArrayAsync();
    }

    public async Task<ICollection<string>> GetPrevTargets(string targetId)
    {
        return await _repository
            .Where(l => l.ToId == targetId)
            .Select(l => l.FromId)
            .GetAllAsync()
            .ToArrayAsync();
    }
}
