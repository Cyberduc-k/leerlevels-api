namespace Model.DTO;

public class McqProgressDTO
{
    public string AnswerOptionId { get; set; }
    public AnswerKind AnswerKind { get; set; }

    public McqProgressDTO()
    {
    }

    public McqProgressDTO(string answerOptionId, AnswerKind answerKind)
    {
        AnswerOptionId = answerOptionId;
        AnswerKind = answerKind;
    }
}
