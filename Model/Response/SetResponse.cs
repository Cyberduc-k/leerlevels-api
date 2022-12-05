namespace Model.Response;

public class SetResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? GroupId { get; set; }
    public ICollection<TargetResponse> Targets { get; set; }
}
