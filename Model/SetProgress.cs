using Model.Response;

namespace Model;

public class SetProgress
{
    public Set Set { get; set; }
    public ICollection<TargetProgress> Targets { get; set; }

    public SetProgress()
    {
    }

    public SetProgress(Set set, ICollection<TargetProgress> targets)
    {
        Set = set;
        Targets = targets;
    }

    public bool IsCompleted() => Targets.All(t => t.IsCompleted());

    public int CalculateScore() => Targets.Sum(t => t.CalculateScore()) / Targets.Count;

    public SetProgressResponse CreateResponse()
    {
        TargetProgressResponse[] targets = Targets.Select(t => t.CreateResponse()).ToArray();

        return new SetProgressResponse(Set.Id, targets, CalculateScore(), IsCompleted());
    }
}
