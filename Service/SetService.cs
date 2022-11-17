using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
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

    private IQueryableRepository<Set> GetAllQuery(int limit, int page) => _setRepository
        .OrderBy(s => s.Name)
        .Skip(limit * page)
        .Limit(limit);

    public async Task<ICollection<Set>> GetAllSetsAsync(int limit, int page)
    {
        return await GetAllQuery(limit, page).GetAllAsync().ToArrayAsync();
    }

    public async Task<ICollection<Set>> GetAllSetsFilteredAsync(int limit, int page, string filter)
    {
        return await GetAllQuery(limit, page).GetAllWhereAsync(s => s.Name.Contains(filter)).ToArrayAsync();
    }

    public async Task<Set> GetSetByIdAsync(string setId)
    {
        return await _setRepository.Include(x => x.Targets).GetByAsync(x => x.Id == setId) ?? throw new NotFoundException("set");
    }
}
