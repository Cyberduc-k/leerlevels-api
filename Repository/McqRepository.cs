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
public class McqRepository : Repository<Mcq>, IMcqRepository
{
    public McqRepository(McqContext context) : base(context, context.Mcqs)
    {
    }

}
