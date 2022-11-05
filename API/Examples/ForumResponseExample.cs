using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class ForumResponseExample : OpenApiExample<ForumResponse>
{
    public override IOpenApiExample<ForumResponse> Build(NamingStrategy namingStrategy)
    {
        ForumReplyResponse reply = new() {
            Id = Guid.NewGuid().ToString(),
            FromId = Guid.NewGuid().ToString(),
            Text = "Example reply",
            Upvotes = 2,
        };

        Examples.Add(OpenApiExampleResolver.Resolve("ForumResponse", new ForumResponse() {
            Id = Guid.NewGuid().ToString(),
            Title = "Example forum",
            Description = "Example description",
            FromId = Guid.NewGuid().ToString(),
            Replies = new[] { reply }
        }, namingStrategy));

        return this;
    }
}
