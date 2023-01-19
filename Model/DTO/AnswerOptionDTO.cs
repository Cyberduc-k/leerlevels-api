using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.DTO;

public class AnswerOptionDTO
{
    [OpenApiProperty(Description = "The id of the answer option", Nullable = true)]
    public string? Id { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The index of this answer option in the list of possible answers", Nullable = false)]
    public int Index { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The text of this answer", Nullable = false)]
    public string Text { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "Whether this answer is correct or not", Nullable = false)]
    public bool IsCorrect { get; set; }
}
