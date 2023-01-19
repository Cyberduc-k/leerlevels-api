using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository;
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

    public async Task<Mcq> CreateMcq(Mcq newMcq)
    {
        newMcq.Id = Guid.NewGuid().ToString();
        
        foreach (AnswerOption answer in newMcq.AnswerOptions)
            answer.Id = Guid.NewGuid().ToString();

        await _mcqRepository.InsertAsync(newMcq);
        await _mcqRepository.SaveChanges();
        return newMcq;
    }

    public async Task UpdateMcq(string McqId, UpdateMcqDTO changes)
    {
        Mcq Mcq = await GetMcqByIdAsync(McqId);
        Mcq.QuestionText = changes.QuestionText ?? Mcq.QuestionText;
        Mcq.Explanation = changes.Explanation ?? Mcq.Explanation;
        Mcq.AllowRandom = changes.AllowRandom ?? Mcq.AllowRandom;

        if (changes.AnswerOptions is not null) {
            var keep = changes.AnswerOptions.Where(c => c.Id is not null).Select(c => c.Id!).ToArray();

            Mcq.AnswerOptions = Mcq.AnswerOptions.Where(a => keep.Contains(a.Id)).Select(a => {
                var change = changes.AnswerOptions.First(c => c.Id == a.Id);
                a.Index = change.Index;
                a.Text = change.Text;
                a.IsCorrect = change.IsCorrect;
                return a;
            }).Concat(changes.AnswerOptions.Where(c => c.Id is null).Select(c => {
                return new AnswerOption(Guid.NewGuid().ToString(), c.Index, c.Text, c.IsCorrect);
            })).ToList();
        }

        await _mcqRepository.SaveChanges();
    }

    public async Task DeleteMcq(string McqId)
    {
        Mcq Mcq = await GetMcqByIdAsync(McqId);

        _mcqRepository.Remove(Mcq);
        await _mcqRepository.SaveChanges();
    }
}
