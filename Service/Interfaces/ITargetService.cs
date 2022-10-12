using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Service.Interfaces;
public interface ITargetService
{
    Task<ICollection<Target>> GetAllTargetsAsync();

    Task<Target> GetTargetByIdAsync(string targetId);
}
