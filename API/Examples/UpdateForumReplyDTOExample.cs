using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class UpdateForumReplyDTOExample : OpenApiExample<UpdateForumReplyDTO>
{
    public override IOpenApiExample<UpdateForumReplyDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("UpdateForumReplyDTO_Text", new UpdateForumReplyDTO() {
            Text = "Example reply",
        }, namingStrategy));

        Examples.Add(OpenApiExampleResolver.Resolve("UpdateForumReplyDTO_Upvotes", new UpdateForumReplyDTO() {
            Upvotes = 2,
        }, namingStrategy));

        Examples.Add(OpenApiExampleResolver.Resolve("UpdateForumReplyDTO_All", new UpdateForumReplyDTO() {
            Text = "Example reply",
            Upvotes = 2,
        }, namingStrategy));

        return this;
    }
}
