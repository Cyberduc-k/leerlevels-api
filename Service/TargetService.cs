using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;

namespace Service;

public class TargetService : ITargetService
{
    private readonly ILogger _logger;
    private readonly ITargetRepository _targetRepository;

    public TargetService(ILoggerFactory loggerFactory, ITargetRepository targetRepository)
    {
        _logger = loggerFactory.CreateLogger<TargetService>();
        _targetRepository = targetRepository;
    }

    public async Task<ICollection<Target>> GetAllTargetsAsync(int limit, int page)
    {
        return await _targetRepository
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.AnswerOptions)
            .OrderBy(x => x.Label)
            .Skip(limit * page)
            .Limit(limit)
            .GetAllAsync()
            .ToArrayAsync();
    }

    public async Task<ICollection<Target>> GetAllTargetsFilteredAsync(int limit, int page, string filter)
    {
        return await _targetRepository
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.AnswerOptions)
            .Where(t => t.Label.Contains(filter))
            .OrderBy(x => x.Label)
            .Skip(limit * page)
            .Limit(limit)
            .GetAllAsync()
            .ToArrayAsync();
    }

    public async Task<Target> GetTargetByIdAsync(string targetId)
    {
        return await _targetRepository
            .Include(t => t.Mcqs)
            .ThenInclude(m => m.AnswerOptions)
            .GetByAsync(t => t.Id == targetId) ?? throw new NotFoundException("target");
    }

    public async Task<int> GetTargetCountAsync()
    {
        return await _targetRepository.CountAsync();
    }

    public async Task<Target> CreateTarget(Target newTarget)
    {
        newTarget.Id = Guid.NewGuid().ToString();
        newTarget.Mcqs = new List<Mcq>();

        await _targetRepository.InsertAsync(newTarget);
        await _targetRepository.SaveChanges();
        return newTarget;
    }

    public async Task UpdateTarget(string targetId, UpdateTargetDTO changes)
    {
        Target target = await GetTargetByIdAsync(targetId);
        target.Label = changes.Label ?? target.Label;
        target.Description = changes.Description ?? target.Description;
        target.TargetExplanation = changes.TargetExplanation ?? target.TargetExplanation;
        target.YoutubeId = changes.YoutubeId ?? target.YoutubeId;
        target.ImageUrl = changes.ImageUrl ?? target.ImageUrl;
        await _targetRepository.SaveChanges();
    }

    public async Task DeleteTarget(string targetId)
    {
        Target target = await GetTargetByIdAsync(targetId);

        _targetRepository.Remove(target);
        await _targetRepository.SaveChanges();
    }
}
