namespace Model;

public class Group
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public EducationType EducationType { get; set; }
    public SchoolYear SchoolYear { get; set; }
    public virtual ICollection<Set> Sets { get; set; }
    public virtual ICollection<User> Users { get; set; }

    public Group()
    {

    }

    public Group(string id, string name, string subject, EducationType educationType, SchoolYear schoolYear, ICollection<Set> set)
    {
        Id = id;
        Name = name;
        Subject = subject;
        EducationType = educationType;
        SchoolYear = schoolYear;
        Sets = set;
    }
}
