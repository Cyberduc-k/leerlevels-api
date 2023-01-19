using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class UpdateGroupDTO
{
    [OpenApiProperty(Description = "The name of this group", Nullable = true)]
    public string? Name { get; set; }

    [OpenApiProperty(Description = "The subject of this group", Nullable = true)]
    public string? Subject { get; set; }

    [OpenApiProperty(Description = "The type of education", Nullable = true)]
    public EducationType? EducationType { get; set; }

    [OpenApiProperty(Description = "The school year", Nullable = true)]
    public SchoolYear? SchoolYear { get; set; }
}
