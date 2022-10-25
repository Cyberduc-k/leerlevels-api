namespace Model.Response;

public class BookmarksResponse
{
    public ICollection<Target> Targets { get; set; }
    public ICollection<McqResponse> Mcqs { get; set; }

    public BookmarksResponse()
    {
    }

    public BookmarksResponse(ICollection<Target> targets, ICollection<McqResponse> mcqs)
    {
        Targets = targets;
        Mcqs = mcqs;
    }
}
