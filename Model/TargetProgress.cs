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

    public bool IsComplete => Mcqs.All(m => m.Answer is not null);
}
