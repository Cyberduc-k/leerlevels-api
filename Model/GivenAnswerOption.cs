using Newtonsoft.Json;

namespace Model;

public class GivenAnswerOption
{
    [JsonIgnore]
    public string AnswerId { get; set; }
    public AnswerOption Answer { get; set; }
    public AnswerKind AnswerKind { get; set; }

    public GivenAnswerOption()
    {
    }

    public GivenAnswerOption(AnswerOption answer, AnswerKind answerKind)
    {
        AnswerId = answer.Id;
        Answer = answer;
        AnswerKind = answerKind;
    }
}
