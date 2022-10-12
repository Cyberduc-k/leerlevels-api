using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Model;
using Repository;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service;
public class TargetService : ITargetService
{
    private readonly ITargetRepository _targetRepository;

    private ILogger _Logger { get; }

    public TargetService(ITargetRepository targetRepository, ILoggerFactory logger)
    {
        _targetRepository = targetRepository;
        _Logger = logger.CreateLogger<TargetService>();
    }

    //get users

    public async Task<ICollection<Target>> GetAllTargetsAsync()
    {
        return await _targetRepository.GetAllAsync().ToArrayAsync();
    }

    public async Task<Target> GetTargetByIdAsync(string targetId)
    {
        return await _targetRepository.GetByIdAsync(targetId) ?? throw new NullReferenceException();
    }
}



