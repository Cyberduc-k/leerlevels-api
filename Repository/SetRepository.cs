using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;
using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Interfaces;

namespace Repository;
public class SetRepository : Repository<Set>, ISetRepository
{
    public SetRepository(DbContext context, DbSet<Set> dbset) : base(context, dbset)
    {
    }
}
