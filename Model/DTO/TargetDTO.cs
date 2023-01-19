using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.DTO;

public class TargetDTO
{
    [JsonRequired]
    [OpenApiProperty(Description = "The label or name of this target", Nullable = false)]
    public string Label { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The description of this target", Nullable = false)]
    public string Description { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The explanation of this target", Nullable = false)]
    public string TargetExplanation { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The id of a youtube video for this target", Nullable = false)]
    public string YoutubeId { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The url to an image for this target", Nullable = false)]
    public string ImageUrl { get; set; }
}
