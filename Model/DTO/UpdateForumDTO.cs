using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class UpdateForumDTO
{
    [OpenApiProperty(Description = "The new title of a forum", Nullable = true)]
    public string? Title { get; set; }

    [OpenApiProperty(Description = "The new description of a forum", Nullable = true)]
    public string? Description { get; set; }
}
