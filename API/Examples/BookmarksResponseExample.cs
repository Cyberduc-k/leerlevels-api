using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class BookmarksResponseExample : OpenApiExample<BookmarksResponse>
{
    public override IOpenApiExample<BookmarksResponse> Build(NamingStrategy namingStrategy)
    {
        TargetResponse target = new() {
            Id = Guid.NewGuid().ToString(),
            Label = "Example label",
            Description = "Example description",
            TargetExplanation = "Example target explanation",
            YoutubeId = "FA9GT_g0P0U",
            ImageUrl = "https://i.imgur.com/pMmtHD1.jpg",
        };

        McqResponse mcq = new() {
            Id = Guid.NewGuid().ToString(),
            TargetId = Guid.NewGuid().ToString(),
            QuestionText = "Example question",
            Explanation = "Example explanation",
            AllowRandom = true,
        };

        Examples.Add(OpenApiExampleResolver.Resolve("Bookmarks", new BookmarksResponse() {
            Targets = new[] { target },
            Mcqs = new[] { mcq },
        }, namingStrategy));

        return this;
    }
}
