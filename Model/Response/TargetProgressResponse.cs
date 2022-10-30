namespace Model.Response;

public class TargetProgressResponse
{
    public string TargetId { get; set; }
    public ICollection<McqProgressResponse> Mcqs { get; set; }
}
