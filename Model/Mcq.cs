namespace Model;

public class Mcq
{
    public string Id { get; set; }
    public Target Target { get; set; }
    public string QuestionText { get; set; }
    public string Explanation { get; set; }
    public bool AllowRandom { get; set; }
    public virtual ICollection<AnswerOption> AnswerOptions { get; set; }

    public Mcq()
    {
    }

    public Mcq(string id, Target target, string questionText, string explanation, bool allowRandom, ICollection<AnswerOption> answerOptions)
    {
        Id = id;
        Target = target;
        QuestionText = questionText;
        Explanation = explanation;
        AllowRandom = allowRandom;
        AnswerOptions = answerOptions;
    }
}
