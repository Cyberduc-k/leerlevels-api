using Newtonsoft.Json;

namespace Model;

public class GivenAnswerOption
{
    public int Id { get; set; }
    public AnswerOption Answer { get; set; }
    public AnswerKind AnswerKind { get; set; }

    public GivenAnswerOption()
    {
    }

    public GivenAnswerOption(AnswerOption answer, AnswerKind answerKind)
    {
        Answer = answer;
        AnswerKind = answerKind;
    }
}
