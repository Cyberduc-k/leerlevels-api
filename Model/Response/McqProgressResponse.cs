namespace Model.Response;

public class McqProgressResponse
{
    public string McqId { get; set; }
    public string? AnswerOptionId { get; set; }
    public AnswerKind? AnswerKind { get; set; }
}
