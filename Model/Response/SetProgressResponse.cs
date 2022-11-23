namespace Model.Response;

public class SetProgressResponse
{
    public string SetId { get; set; }
    public ICollection<TargetProgressResponse> Targets { get; set; }
    public int Score { get; set; }
    public bool IsCompleted { get; set; }

    public SetProgressResponse()
    {
    }

    public SetProgressResponse(string setId, ICollection<TargetProgressResponse> targets, int score, bool isCompleted)
    {
        SetId = setId;
        Targets = targets;
        Score = score;
        IsCompleted = isCompleted;
    }
}
