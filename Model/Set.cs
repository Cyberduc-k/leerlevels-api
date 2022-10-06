using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model;
public class Set
{
    public string Id { get; set; } 
    public virtual ICollection<Target> Targets { get; set; }    

    public Set()
    {

    }
    public Set(string id, ICollection<Target> targets)
    {
        Id = id;
        Targets = targets;
    }   
}
