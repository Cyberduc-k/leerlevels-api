namespace Model.Response;
public class CreateGroupResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public EducationType EducationType { get; set; }
    public SchoolYear SchoolYear { get; set; }
}