using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model;
public class Group
{
    public string Id { get; set; } 
    public string Name { get; set; }    
    public string Subject { get; set; }
    public EducationType educationType { get; set; }
    public SchoolYear schoolYear { get; set; }
    public virtual ICollection<Set> Set { get; set; }

    public Group()
    {

    }

    public Group(string id, string name, string subject, EducationType educationType, SchoolYear schoolYear, ICollection<Set> set)
    {
        Id = id;
        Name = name;
        Subject = subject;
        this.educationType = educationType;
        this.schoolYear = schoolYear;
        Set = set;
    }   
}
