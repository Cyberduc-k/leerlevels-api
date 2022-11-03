using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
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

    public async Task<ICollection<Set>> GetAllSetsAsync()
    {
        return await _setRepository.Include(x => x.Targets).GetAllAsync().ToArrayAsync() ?? throw new Exception("Inernal Server Error");
    }

    public async Task<Set> GetSetByIdAsync(string setId)
    {
        return await _setRepository.Include(x =>x.Targets).GetByAsync(x =>x.Id == setId) ?? throw new NotFoundException("set");
    }
}
