using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model;
public class Forum
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public virtual ICollection<User> From { get; set; } 

    public Forum()
    {

    }

    public Forum(string id, string title, string description, ICollection<User> from)
    {
        Id = id;
        Title = title;
        Description = description;
        From = from;
    }   
}
