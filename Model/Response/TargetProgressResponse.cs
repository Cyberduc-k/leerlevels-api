namespace Model.Response;

public class TargetProgressResponse
{
    public string TargetId { get; set; }
    public ICollection<McqProgressResponse> Mcqs { get; set; }
    public double Score { get; set; }
    public bool IsCompleted { get; set; }

    public TargetProgressResponse()
    {
    }

    public TargetProgressResponse(string targetId, ICollection<McqProgressResponse> mcqs, double score, bool isCompleted)
    {
        TargetId = targetId;
        Mcqs = mcqs;
        Score = score;
        IsCompleted = isCompleted;
    }
}
