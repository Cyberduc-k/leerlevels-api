using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Model;
using Repository.Interfaces;

namespace Repository;
public class TargetRepository : Repository<Target>, ITargetRepository
{
    public TargetRepository(TargetContext context) : base(context, context.Targets)
    {
    }
}
