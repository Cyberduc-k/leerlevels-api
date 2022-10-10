using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
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

    public Task<IEnumerable<Mcq>> GetAllAsync()
    {
        return mcqRepository.GetAllAsync();
    }

    public Task<Mcq> GetByIdAsync(string mcqId)
    {
        throw new NotImplementedException();
    }
}
