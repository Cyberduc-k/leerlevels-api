namespace Model.Response;

public class TargetProgressResponse
{
    public string TargetId { get; set; }
    public ICollection<McqProgressResponse> Mcqs { get; set; }
    public int Score { get; set; }
    public int NumberOfCorrectAnswers { get; set; }
    public bool IsCompleted { get; set; }

    public TargetProgressResponse()
    {
    }

    public TargetProgressResponse(string targetId, ICollection<McqProgress> mcqs, int score, bool isCompleted)
    {
        Mcqs = mcqs.Select(m => m.CreateResponse()).ToArray();
        NumberOfCorrectAnswers = mcqs.Where(m => m.Answers is ICollection<GivenAnswerOption> a && a.Any(a => a.Answer.IsCorrect)).Count();
        TargetId = targetId;
        Score = score;
        IsCompleted = isCompleted;
    }
}
