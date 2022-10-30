namespace Model.Response;

public class McqProgressResponse
{
    public string McqId { get; set; }
    public string? AnswerOptionId { get; set; }
    public AnswerKind? AnswerKind { get; set; }
    public double Score { get; set; }

    public McqProgressResponse()
    {
    }

    public McqProgressResponse(string mcqId, string? answerOptionId, AnswerKind? answerKind, double score)
    {
        McqId = mcqId;
        AnswerOptionId = answerOptionId;
        AnswerKind = answerKind;
        Score = score;
    }
}
