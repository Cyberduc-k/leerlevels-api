using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class UpdateForumReplyDTO
{
    [OpenApiProperty(Description = "The new text of a forum reply", Nullable = false)]
    public string? Text { get; set; }

    [OpenApiProperty(Description = "The new amount of upvotes a forum reply has", Nullable = true)]
    public int? Upvotes { get; set; }
}
