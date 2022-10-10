using Newtonsoft.Json;

namespace Model.DTO;

public class ForumDTO
{
    [JsonRequired]
    public string Title { get; set; }

    [JsonRequired]
    public string Description { get; set; }

    [JsonRequired]
    public string FromId { get; set; }

    public ForumDTO()
    {
    }

    public ForumDTO(string title, string description, string fromId)
    {
        Title = title;
        Description = description;
        FromId = fromId;
    }
}
