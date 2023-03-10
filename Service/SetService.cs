using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using Model.DTO;
using Repository;
using Repository.Interfaces;
using Service.Exceptions;
using Service.Interfaces;

namespace Service;

public class SetService : ISetService
{
    private readonly ILogger _logger;
    private readonly ISetRepository _setRepository;

    public SetService(ILoggerFactory loggerFactory, ISetRepository setRepository)
    {
        _logger = loggerFactory.CreateLogger<SetService>();
        _setRepository = setRepository;
    }

    public async Task<ICollection<Set>> GetAllSetsAsync(int limit, int page)
    {
        return await _setRepository
            .Include(s => s.Targets)
            .ThenInclude(t => t.Mcqs)
            .ThenInclude(m => m.AnswerOptions)
            .OrderBy(s => s.Name)
            .Skip(limit * page)
            .Limit(limit)
            .GetAllAsync()
            .ToArrayAsync();
    }

    public async Task<ICollection<Set>> GetAllSetsFilteredAsync(int limit, int page, string filter)
    {
        return await _setRepository
            .Include(s => s.Targets)
            .ThenInclude(t => t.Mcqs)
            .ThenInclude(m => m.AnswerOptions)
            .Where(s => s.Name.Contains(filter))
            .OrderBy(s => s.Name)
            .Skip(limit * page)
            .Limit(limit)
            .GetAllAsync()
            .ToArrayAsync();
    }

    public async Task<Set> GetSetByIdAsync(string setId)
    {
        return await _setRepository
            .Include(x => x.Targets)
            .ThenInclude(t => t.Mcqs)
            .ThenInclude(m => m.AnswerOptions)
            .Include(x => x.Group)
            .GetByAsync(x => x.Id == setId) ?? throw new NotFoundException("set");
    }

    public async Task<Set> CreateSet(Set newSet)
    {
        newSet.Id = Guid.NewGuid().ToString();
        newSet.Targets = new List<Target>();

        await _setRepository.InsertAsync(newSet);
        await _setRepository.SaveChanges();
        return newSet;
    }

    public async Task UpdateSet(string setId, UpdateSetDTO changes)
    {
        Set set = await GetSetByIdAsync(setId);
        set.Name = changes.Name ?? set.Name;
        await _setRepository.SaveChanges();
    }

    public async Task DeleteSet(string SetId)
    {
        Set Set = await GetSetByIdAsync(SetId);

        _setRepository.Remove(Set);
        await _setRepository.SaveChanges();
    }
}
