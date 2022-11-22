using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class SetProgressResponseExample : OpenApiExample<SetProgressResponse>
{
    public override IOpenApiExample<SetProgressResponse> Build(NamingStrategy namingStrategy)
    {
        AnswerOption answer = new(Guid.NewGuid().ToString(), 0, "Example answer", true);

        McqProgressResponse mcqProgress = new() {
            McqId = Guid.NewGuid().ToString(),
            Answers = new GivenAnswerOption[] {
                new(answer, AnswerKind.NotSure),
            },
        };

        TargetProgressResponse targetProgress = new() {
            TargetId = Guid.NewGuid().ToString(),
            Score = 60,
            IsCompleted = true,
            Mcqs = new[] { mcqProgress },
        };

        Examples.Add(OpenApiExampleResolver.Resolve("SetProgressResponse", new SetProgressResponse() {
            SetId = Guid.NewGuid().ToString(),
            Score = 60,
            IsCompleted = true,
            Targets = new[] { targetProgress },
        }, namingStrategy));

        return this;
    }
}
