namespace Model.Response;

public class McqProgressResponse
{
    public string McqId { get; set; }
    public ICollection<GivenAnswerOption> Answers { get; set; }

    public McqProgressResponse()
    {
    }

    public McqProgressResponse(string mcqId, ICollection<GivenAnswerOption> answers)
    {
        McqId = mcqId;
        Answers = answers;
    }
}
