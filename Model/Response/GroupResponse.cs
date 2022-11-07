using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Response;
public class GroupResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public EducationType EducationType { get; set; }
    public SchoolYear SchoolYear { get; set; }
    public ICollection<UserResponse> Users { get; set; }   
    public ICollection<SetResponse> Sets { get; set; }
}
