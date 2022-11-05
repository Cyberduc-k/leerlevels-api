using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class TargetProgressResponseExample : OpenApiExample<TargetProgressResponse>
{
    public override IOpenApiExample<TargetProgressResponse> Build(NamingStrategy namingStrategy)
    {
        McqProgressResponse mcqProgress = new() {
            McqId = Guid.NewGuid().ToString(),
            AnswerOptionId = Guid.NewGuid().ToString(),
            AnswerKind = Model.AnswerKind.NotSure,
            Score = 0.6,
        };

        Examples.Add(OpenApiExampleResolver.Resolve("TargetProgressResponse", new TargetProgressResponse() {
            TargetId = Guid.NewGuid().ToString(),
            Score = 0.6,
            IsCompleted = true,
            Mcqs = new[] { mcqProgress }
        }, namingStrategy));

        return this;
    }
}
