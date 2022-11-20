namespace Model.Response;

public class McqResponse
{
    public string Id { get; set; }
    public bool? IsBookmarked { get; set; }
    public string TargetId { get; set; }
    public string QuestionText { get; set; }
    public string Explanation { get; set; }
    public bool AllowRandom { get; set; }
    public virtual ICollection<AnswerOption> AnswerOptions { get; set; }

    public McqResponse()
    {
    }

    public McqResponse(string id, string targetId, string questionText, string explanation, bool allowRandom, ICollection<AnswerOption> answerOptions, bool isbookedmarked)
    {
        Id = id;
        TargetId = targetId;
        QuestionText = questionText;
        Explanation = explanation;
        AllowRandom = allowRandom;
        AnswerOptions = answerOptions;
        IsBookmarked = isbookedmarked;
    }
}
