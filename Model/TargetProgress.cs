using Model.Response;

namespace Model;

public class TargetProgress
{
    public int Id { get; set; }
    public User User { get; set; }
    public Target Target { get; set; }
    public virtual ICollection<McqProgress> Mcqs { get; set; }

    public TargetProgress()
    {
        Mcqs = new List<McqProgress>();
    }

    public TargetProgress(int id, User user, Target target, ICollection<McqProgress> mcqs)
    {
        Id = id;
        User = user;
        Target = target;
        Mcqs = mcqs;
    }

    public bool IsCompleted() => CalculateScore() >= 65;

    public int CalculateScore()
    {
        double total = Mcqs.Sum(McqScore);
        return Mcqs.Count == 0 ? 0 : (int)Math.Round(total / Mcqs.Count);
    }

    private double McqScore(McqProgress mcq)
    {
        if (mcq.Answers.Count == 0) return 0;
        double score = mcq.Answers.Sum(AnswerScore);
        int numAnswers = mcq.Answers.Count;
        int maxScore = Enum.GetValues<AnswerKind>().Length;
        return 100 * (0.5 + score / (2 * maxScore * numAnswers));
    }

    private int AnswerScore(GivenAnswerOption answer)
    {
        int score = answer.AnswerKind switch {
            AnswerKind.Sure => 3,
            AnswerKind.NotSure => 2,
            AnswerKind.Guess => 1,
            _ => throw new Exception("Invalid AnswerKind"),
        };

        return answer.Answer.IsCorrect ? score : -score;
    }

    public TargetProgressResponse CreateResponse()
    {
        return new TargetProgressResponse(Target.Id, Mcqs, CalculateScore(), IsCompleted());
    }
}
