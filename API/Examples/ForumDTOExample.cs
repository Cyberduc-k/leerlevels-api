using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class ForumDTOExample : OpenApiExample<ForumDTO>
{
    public override IOpenApiExample<ForumDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("ForumDTO", new ForumDTO() {
            Title = "Example forum",
            Description = "Example description",
        }, namingStrategy));

        return this;
    }
}
