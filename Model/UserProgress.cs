using Model.Response;

namespace Model;

public class UserProgress
{
    public ICollection<TargetProgress> Targets { get; set; }
    public ICollection<SetProgress> Sets { get; set; }
    public int TotalTargets { get; set; }

    public UserProgress()
    {
    }

    public UserProgress(ICollection<TargetProgress> targets, ICollection<SetProgress> sets, int totalTargets)
    {
        Targets = targets;
        Sets = sets;
        TotalTargets = totalTargets;
    }

    public UserProgressResponse CreateResponse()
    {
        int targetsCompleted = (int)Math.Round((double)Targets.Where(t => t.IsCompleted()).Count() / TotalTargets * 100);
        int setsCompleted = (int)Math.Round((double)Sets.Where(s => s.IsCompleted()).Count() / Sets.Count * 100);
        int averageScore = (int)Math.Round((double)Targets.Sum(t => t.CalculateScore()) / Targets.Count);

        return new UserProgressResponse(targetsCompleted, setsCompleted, averageScore);
    }
}
