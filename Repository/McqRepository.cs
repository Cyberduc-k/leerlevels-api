using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Interfaces;

namespace Repository;
public class McqRepository : IMcqRepository
{
    private readonly TargetContext targetContext;

    public McqRepository(TargetContext targetContext)
    {
        this.targetContext = targetContext;
    }   

    public Task<IEnumerable<Mcq>> GetAllAsync()
    {
        throw new NotImplementedException();

    }

    public Task<Mcq> GetByIdAsync(string mcqId)
    {
        throw new NotImplementedException();
    }
}
