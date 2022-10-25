namespace Model.Response;

public class BookmarksResponse
{
    public ICollection<TargetResponse> Targets { get; set; }
    public ICollection<McqResponse> Mcqs { get; set; }

    public BookmarksResponse()
    {
    }

    public BookmarksResponse(ICollection<TargetResponse> targets, ICollection<McqResponse> mcqs)
    {
        Targets = targets;
        Mcqs = mcqs;
    }
}
