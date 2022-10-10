using Newtonsoft.Json;

namespace Model.Response;

public class ForumResponse
{
    [JsonRequired]
    public string Id { get; set; }

    [JsonRequired]
    public string Title { get; set; }

    [JsonRequired]
    public string Description { get; set; }

    [JsonRequired]
    public string FromId { get; set; }

    [JsonRequired]
    public ICollection<ForumReplyResponse> Replies { get; set; }

    public ForumResponse()
    {
    }

    public ForumResponse(string id, string title, string description, string fromId, ICollection<ForumReplyResponse> replies)
    {
        Id = id;
        Title = title;
        Description = description;
        FromId = fromId;
        Replies = replies;
    }
}
