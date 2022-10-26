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

    public async Task<ICollection<Target>> GetAllTargetsAsync()
    {
        return await _targetRepository.GetAllIncludingAsync(x => x.Mcqs).ToArrayAsync();
    }

    public async Task<Target> GetTargetByIdAsync(string targetId)
    {
        return await _targetRepository.GetByIdAsync(targetId) ?? throw new NotFoundException("target");
    }
}
