using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.DTO;

public class GroupDTO
{
    [JsonRequired]
    [OpenApiProperty(Description = "The name of this group", Nullable = false)]
    public string Name { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The subject of this group", Nullable = false)]
    public string Subject { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The type of education", Nullable = false)]
    public EducationType EducationType { get; set; }

    [JsonRequired]
    [OpenApiProperty(Description = "The school year", Nullable = false)]
    public SchoolYear SchoolYear { get; set; }
}
