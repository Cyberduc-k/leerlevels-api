using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Service.Interfaces;
public interface IMcqService
{
    Task<ICollection<Mcq>> GetAllMcqsAsync();

    Task<Mcq> GetMcqByIdAsync(string mcqId);
}
