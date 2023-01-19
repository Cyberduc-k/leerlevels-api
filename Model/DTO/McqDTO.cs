using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.DTO;

public class McqDTO
{
    [JsonRequired]
    [OpenApiProperty(Description = "The text of this question", Nullable = false)]
    public string QuestionText { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The explanation for the correct answer", Nullable = false)]
    public string Explanation { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "Whether a guess is allowed or not", Nullable = false)]
    public bool AllowRandom { get; set; }

    [OpenApiProperty(Description = "The list of possible answers", Nullable = true)]
    public AnswerOptionDTO[]? AnswerOptions { get; set; }
}
