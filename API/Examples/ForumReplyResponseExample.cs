using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class ForumReplyResponseExample : OpenApiExample<ForumReplyResponse>
{
    public override IOpenApiExample<ForumReplyResponse> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("ForumReplyResponse", new ForumReplyResponse() {
            Id = Guid.NewGuid().ToString(),
            FromId = Guid.NewGuid().ToString(),
            Text = "Example reply",
            Upvotes = 2,
        }, namingStrategy));

        return this;
    }
}
