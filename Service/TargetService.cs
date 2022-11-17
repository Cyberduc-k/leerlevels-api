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
            .Include(x => x.Mcqs)
            .ThenInclude(m => m.AnswerOptions)
            .OrderBy(t => t.Label)
            .Skip(limit * page)
            .Limit(limit)
            .GetAllAsync()
            .ToArrayAsync();
    }

    public async Task<ICollection<Target>> GetAllTargetsFilteredAsync(int limit, int page, string filter)
    {
        return await _targetRepository
            .Include(x => x.Mcqs)
            .ThenInclude(m => m.AnswerOptions)
            .OrderBy(t => t.Label)
            .Skip(limit * page)
            .Limit(limit)
            .GetAllWhereAsync(t => t.Label.Contains(filter))
            .ToArrayAsync();
    }

    public async Task<Target> GetTargetByIdAsync(string targetId)
    {
        return await _targetRepository.GetByIdAsync(targetId) ?? throw new NotFoundException("target");
    }

    public async Task<Target> GetTargetWithMcqByIdAsync(string targetId)
    {
        return await _targetRepository.Include(t => t.Mcqs).GetByAsync(t => t.Id == targetId)
            ?? throw new NotFoundException("target");
    }
}
