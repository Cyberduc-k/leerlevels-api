namespace Model;

public class Target
{
    public string Id { get; set; }
    public string Label { get; set; }
    public string Description { get; set; }
    public string TargetExplanation { get; set; }
    public string YoutubeId { get; set; }
    public string ImageUrl { get; set; }
    public virtual ICollection<string> Mcqs { get; set; }

    public Target()
    {
    }

    public Target(string id, string label, string description, string targetExplanation, string youtubeId, string imageUrl, ICollection<string> mcqs)
    {
        Id = id;
        Label = label;
        Description = description;
        TargetExplanation = targetExplanation;
        YoutubeId = youtubeId;
        ImageUrl = imageUrl;
        Mcqs = mcqs;
    }
}
