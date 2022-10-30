using Model.Response;

namespace Model;

public class McqProgress
{
    public int Id { get; set; }
    public User User { get; set; }
    public Mcq Mcq { get; set; }
    public AnswerOption? Answer { get; set; }
    public AnswerKind? AnswerKind { get; set; }

    public McqProgress()
    {
    }

    public McqProgress(int id, User user, Mcq mcq, AnswerOption? answer, AnswerKind? answerKind)
    {
        Id = id;
        User = user;
        Mcq = mcq;
        Answer = answer;
        AnswerKind = answerKind;
    }

    public double CalculateScore()
    {
        if (AnswerKind is AnswerKind kind) {
            if (Answer!.IsCorrect)
                return kind switch {
                    Model.AnswerKind.Sure => 1,
                    Model.AnswerKind.NotSure => 0.6,
                    Model.AnswerKind.Guess => 0.3,
                };
        }

        return 0;
    }

    public McqProgressResponse CreateResponse()
    {
        return new McqProgressResponse(Mcq.Id, Answer?.Id, AnswerKind, CalculateScore());
    }
}
