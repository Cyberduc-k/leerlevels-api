namespace Model.Response;

public class SetProgressResponse
{
    public string SetId { get; set; }
    public ICollection<TargetProgressResponse> Targets { get; set; }
    public double Score { get; set; }
    public bool IsCompleted { get; set; }

    public SetProgressResponse()
    {
    }

    public SetProgressResponse(string setId, ICollection<TargetProgressResponse> targets, double score, bool isCompleted)
    {
        SetId = setId;
        Targets = targets;
        Score = score;
        IsCompleted = isCompleted;
    }
}
