using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.DTO;

public class ForumReplyDTO
{
    [OpenApiProperty(Nullable = true)]
    public string FromId { get; set; }

    [JsonRequired]
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
