namespace Model.DTO;

public class McqDTO
{
    public string Id { get; set; }
    public string TargetId { get; set; }
    public string QuestionText { get; set; }
    public string Explanation { get; set; }
    public bool AllowRandom { get; set; }
    public virtual ICollection<AnswerOption> AnswerOptions { get; set; }

    public McqDTO()
    {
    }

    public McqDTO(string id, string targetId, string questionText, string explanation, bool allowRandom, ICollection<AnswerOption> answerOptions)
    {
        Id = id;
        TargetId = targetId;
        QuestionText = questionText;
        Explanation = explanation;
        AllowRandom = allowRandom;
        AnswerOptions = answerOptions;
    }
}
