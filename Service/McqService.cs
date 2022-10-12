using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Repository;
using Repository.Interfaces;
using Service.Interfaces;

namespace Service;
public class McqService : IMcqService
{
    private readonly IMcqRepository mcqRepository;

    public McqService(IMcqRepository mcqRepository)
    {
        this.mcqRepository = mcqRepository;
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
