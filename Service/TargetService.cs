using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
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
}
