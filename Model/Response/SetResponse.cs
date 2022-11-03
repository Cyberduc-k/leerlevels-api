using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Response;
public class SetResponse
{
    public string Id { get; set; } 
    public ICollection<TargetResponse> Targets { get; set; }
}
