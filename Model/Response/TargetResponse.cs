namespace Model.Response;

public class TargetResponse
{
    public string Id { get; set; }
    public bool? IsBookedmarked { get; set; }
    public string Label { get; set; }
    public string Description { get; set; }
    public string TargetExplanation { get; set; }
    public string YoutubeId { get; set; }
    public string ImageUrl { get; set; }
    public ICollection<McqResponse> Mcqs { get; set; }

    public TargetResponse()
    {
    }

    public TargetResponse(string id, string label, string description, string targetExplanation, string youtubeId, string imageUrl, ICollection<McqResponse> mcqs)
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
