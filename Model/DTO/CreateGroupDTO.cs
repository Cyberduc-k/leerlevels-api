using Model;
public class CreateGroupDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public EducationType EducationType { get; set; }
    public SchoolYear SchoolYear { get; set; }

    public CreateGroupDTO()
    {

    }

    public CreateGroupDTO(string id, string name, string subject, EducationType educationType, SchoolYear schoolYear, ICollection<Set> sets, ICollection<User> users)
    {
        Id = id;
        Name = name;
        Subject = subject;
        EducationType = educationType;
        SchoolYear = schoolYear;
    }
}
