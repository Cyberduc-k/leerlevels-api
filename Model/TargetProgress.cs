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

    public bool IsCompleted => Mcqs.All(m => m.Answer is not null);

    public double CalculateScore()
    {
        return Mcqs.Sum(m => m.CalculateScore()) / Mcqs.Count();
    }

    public TargetProgressResponse CreateResponse()
    {
        McqProgressResponse[] mcqs = Mcqs.Select(m => m.CreateResponse()).ToArray();

        return new TargetProgressResponse(Target.Id, mcqs, CalculateScore(), IsCompleted);
    }
}
