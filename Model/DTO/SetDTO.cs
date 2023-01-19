using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Model.DTO;

public class SetDTO
{
    [JsonRequired]
    [OpenApiProperty(Description = "The name of this set", Nullable = false)]
    public string Name { get; set; }
}
