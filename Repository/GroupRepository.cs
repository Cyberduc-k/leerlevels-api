using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Repository.Interfaces;

namespace Repository;
public class GroupRepository : Repository<Group>, IGroupRepository
{
    public GroupRepository(DbContext context, DbSet<Group> dbset) : base(context, dbset)
    {
    }
}
