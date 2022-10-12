using Microsoft.Extensions.Logging;
using Model;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service;
public class SetService : ISetService
{

    private readonly ISetRepository _setRepository;

    private ILogger _Logger { get; }

    public SetService(ISetRepository setRepository, ILoggerFactory logger)
    {
        _setRepository = setRepository;
        _Logger = logger.CreateLogger<SetService>();
    }
    public async Task<ICollection<Set>> GetAllSetsAsync()
    {
        return await _setRepository.GetAllAsync().ToArrayAsync();
    }

    public async Task<Set> GetSetByIdAsync(string setId)
    {
        return await _setRepository.GetByIdAsync(setId) ?? throw new NullReferenceException();
    }
}
