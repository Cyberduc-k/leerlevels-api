using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public class UpdateSetDTO
{
    [OpenApiProperty(Description = "The name of this set", Nullable = true)]
    public string? Name { get; set; }
}
