using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.DTO;

public class ForumDTO
{
    [OpenApiProperty(Nullable = true)]
    public string FromId { get; set; }

    [JsonRequired]
    public string Title { get; set; }

    [JsonRequired]
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
