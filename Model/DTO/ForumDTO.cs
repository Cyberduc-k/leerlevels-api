using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.DTO;

public class ForumDTO
{
    [OpenApiProperty(Description = "The id of a user posting a forum", Nullable = true)]
    public string FromId { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The title of a new forum", Nullable = false)]
    public string Title { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The description of a new forum", Nullable = false)]
    public string Description { get; set; }

    public ForumDTO()
    {
    }

    public ForumDTO(string fromId, string title, string description)
    {
        FromId = fromId;
        Title = title;
        Description = description;
    }
}
