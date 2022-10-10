using Newtonsoft.Json;

namespace Model.Response;

public class ForumReplyResponse
{
    [JsonRequired]
    public string Id { get; set; }

    [JsonRequired]
    public string FromId { get; set; }

    [JsonRequired]
    public string Text { get; set; }

    public ForumReplyResponse()
    {
    }

    public ForumReplyResponse(string id, string fromId, string text)
    {
        Id = id;
        FromId = fromId;
        Text = text;
    }
}
