using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;

namespace Service;

public class McqService : IMcqService
{
    private readonly ILogger _logger;
    private readonly IMcqRepository _mcqRepository;

    public McqService(ILoggerFactory loggerFactory, IMcqRepository mcqRepository)
    {
        _logger = loggerFactory.CreateLogger<McqService>();
        _mcqRepository = mcqRepository;
    }

    public async Task<ICollection<Mcq>> GetAllMcqsAsync(int limit, int page)
    {
        return await _mcqRepository
            .Include(m => m.AnswerOptions)
            .OrderBy(m => m.Id)
            .Skip(limit * page)
            .Limit(limit)
            .GetAllAsync()
            .ToArrayAsync();
    }

    public async Task<ICollection<Mcq>> GetAllMcqsFilteredAsync(int limit, int page, string filter)
    {
        return await _mcqRepository
            .Include(m => m.AnswerOptions)
            .Where(m => m.QuestionText.Contains(filter))
            .OrderBy(m => m.Id)
            .Skip(limit * page)
            .Limit(limit)
            .GetAllAsync()
            .ToArrayAsync();
    }

    public async Task<Mcq> GetMcqByIdAsync(string mcqId)
    {
        return await _mcqRepository
            .Include(m => m.Target)
            .Include(m => m.AnswerOptions)
            .GetByAsync(m => m.Id == mcqId) ?? throw new NotFoundException("multiple choice question");
    }
}
