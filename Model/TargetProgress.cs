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
        double score = 0;
        double mcqScore = 100.0 / Mcqs.Count;
        double answerScore = 100.0 / Mcqs.Sum(m => m.Answers.Count);

        foreach (McqProgress mcq in Mcqs) {
            if (mcq.Answers is not null) {
                foreach (GivenAnswerOption answer in mcq.Answers) {
                    if (answer.Answer.IsCorrect) {
                        score += answer.AnswerKind switch {
                            AnswerKind.Sure => mcqScore,
                            AnswerKind.NotSure => mcqScore - 5,
                            AnswerKind.Guess => mcqScore - 7,
                            _ => throw new NotImplementedException(),
                        };
                    } else {
                        score += 0.1 * answerScore - answer.AnswerKind switch {
                            AnswerKind.Sure => 10,
                            AnswerKind.NotSure => 5,
                            AnswerKind.Guess => 2,
                            _ => throw new NotImplementedException(),
                        };
                    }
                }
            }
        }

        return (int)Math.Round(score);

    }

    public TargetProgressResponse CreateResponse()
    {
        return new TargetProgressResponse(Target.Id, Mcqs, CalculateScore(), IsCompleted());
    }
}
