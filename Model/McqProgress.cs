using Model.Response;

namespace Model;

public class McqProgress
{
    public int Id { get; set; }
    public User User { get; set; }
    public Mcq Mcq { get; set; }
    public ICollection<GivenAnswerOption> Answers { get; set; }

    public McqProgress()
    {
        Answers = new List<GivenAnswerOption>();
    }

    public McqProgress(int id, User user, Mcq mcq)
    {
        Id = id;
        User = user;
        Mcq = mcq;
        Answers = new List<GivenAnswerOption>();
    }

    public bool AddAnswer(AnswerOption answer, AnswerKind kind)
    {
        if (Answers.FirstOrDefault(a => a.AnswerId == answer.Id && a.AnswerKind == kind) is not null) return false;
        if (Answers.FirstOrDefault(a => a.Answer.IsCorrect) is not null) return false;
        Answers.Add(new(answer, kind));
        return true;
    }

    public McqProgressResponse CreateResponse()
    {
        return new McqProgressResponse(Mcq.Id, Answers);
    }
}
