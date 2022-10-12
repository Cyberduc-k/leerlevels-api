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
public class McqService : IMcqService
{
    private readonly IMcqRepository mcqRepository;

    private ILogger _Logger { get; }

    public McqService(IMcqRepository mcqRepository , ILoggerFactory logger)
    {
        this.mcqRepository = mcqRepository;
        _Logger = logger.CreateLogger<McqService>();
    }   

    public async Task<ICollection<Mcq>> GetAllMcqsAsync()
    {
        return await mcqRepository.GetAllAsync().ToArrayAsync();
    }

    public async Task<Mcq> GetMcqByIdAsync(string mcqId)
    {
        return await mcqRepository.GetByIdAsync(mcqId) ?? throw new NullReferenceException();
    }
}
