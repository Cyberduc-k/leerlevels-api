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
    }

    public McqProgress(int id, User user, Mcq mcq)
    {
        Id = id;
        User = user;
        Mcq = mcq;
        Answers = new List<GivenAnswerOption>();
    }

    public void AddAnswer(AnswerOption answer, AnswerKind kind)
    {
        Answers.Add(new(answer, kind));
    }

    public McqProgressResponse CreateResponse()
    {
        return new McqProgressResponse(Mcq.Id, Answers);
    }
}
