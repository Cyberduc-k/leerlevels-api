using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class ForumReplyDTOExample : OpenApiExample<ForumReplyDTO>
{
    public override IOpenApiExample<ForumReplyDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("ForumReplyDTO", new ForumReplyDTO() {
            Text = "Example reply",
        }, namingStrategy));

        return this;
    }
}
