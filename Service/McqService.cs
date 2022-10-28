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

    public async Task<ICollection<Mcq>> GetAllMcqsAsync()
    {
        return await _mcqRepository.Include(m => m.AnswerOptions).GetAllAsync().ToArrayAsync();
    }

    public async Task<Mcq> GetMcqByIdAsync(string mcqId)
    {
        return await _mcqRepository.GetByIdAsync(mcqId) ?? throw new NotFoundException("multiple choice question");
    }
}
