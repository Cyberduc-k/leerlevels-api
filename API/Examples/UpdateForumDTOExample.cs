using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class UpdateForumDTOExample : OpenApiExample<UpdateForumDTO>
{
    public override IOpenApiExample<UpdateForumDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("UpdateForumDTO_Title", new UpdateForumDTO() {
            Title = "Example title",
        }, namingStrategy));

        Examples.Add(OpenApiExampleResolver.Resolve("UpdateForumDTO_Description", new UpdateForumDTO() {
            Description = "Example description",
        }, namingStrategy));

        Examples.Add(OpenApiExampleResolver.Resolve("UpdateForumDTO_All", new UpdateForumDTO() {
            Title = "Example title",
            Description = "Example description",
        }, namingStrategy));

        return this;
    }
}
