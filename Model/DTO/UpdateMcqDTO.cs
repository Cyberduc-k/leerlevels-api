using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class UpdateMcqDTO
{
    [OpenApiProperty(Description = "The text of this question", Nullable = true)]
    public string? QuestionText { get; set; }

    [OpenApiProperty(Description = "The explanation for the correct answer", Nullable = true)]
    public string? Explanation { get; set; }

    [OpenApiProperty(Description = "Whether a guess is allowed or not", Nullable = true)]
    public bool? AllowRandom { get; set; }

    [OpenApiProperty(Description = "The list of possible answers", Nullable = true)]
    public List<AnswerOptionDTO>? AnswerOptions { get; set; }
}
