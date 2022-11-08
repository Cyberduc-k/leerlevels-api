using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class McqProgressDTO
{
    [OpenApiProperty(Description = "The answer option id of a multiple choice question in progress", Nullable = false)]
    public string AnswerOptionId { get; set; }

    [OpenApiProperty(Description = "The kind of answer type of a multiple choice question in progress", Nullable = false)]
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
