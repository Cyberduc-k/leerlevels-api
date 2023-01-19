using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.DTO;

public class ForumReplyDTO
{
    [OpenApiProperty(Description = "The id of a user posting a forum reply", Nullable = true)]
    public string? FromId { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The text of a forum reply", Nullable = false)]
    public string Text { get; set; }

    public ForumReplyDTO()
    {
    }

    public ForumReplyDTO(string fromId, string text)
    {
        FromId = fromId;
        Text = text;
    }
}
