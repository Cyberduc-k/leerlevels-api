using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class TargetProgressResponseExample : OpenApiExample<TargetProgressResponse>
{
    public override IOpenApiExample<TargetProgressResponse> Build(NamingStrategy namingStrategy)
    {
        AnswerOption answer = new(Guid.NewGuid().ToString(), 0, "Example answer", true);

        McqProgressResponse mcqProgress = new() {
            McqId = Guid.NewGuid().ToString(),
            Answers = new GivenAnswerOption[] {
                new(answer, AnswerKind.NotSure),
            },
        };

        Examples.Add(OpenApiExampleResolver.Resolve("TargetProgressResponse", new TargetProgressResponse() {
            TargetId = Guid.NewGuid().ToString(),
            Score = 60,
            IsCompleted = true,
            Mcqs = new[] { mcqProgress }
        }, namingStrategy));

        return this;
    }
}
