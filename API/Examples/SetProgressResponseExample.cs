using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class SetProgressResponseExample : OpenApiExample<SetProgressResponse>
{
    public override IOpenApiExample<SetProgressResponse> Build(NamingStrategy namingStrategy)
    {
        McqProgressResponse mcqProgress = new() {
            McqId = Guid.NewGuid().ToString(),
            AnswerOptionId = Guid.NewGuid().ToString(),
            AnswerKind = Model.AnswerKind.NotSure,
            Score = 0.6,
        };

        TargetProgressResponse targetProgress = new() {
            TargetId = Guid.NewGuid().ToString(),
            Score = 0.6,
            IsCompleted = true,
            Mcqs = new[] { mcqProgress },
        };

        Examples.Add(OpenApiExampleResolver.Resolve("SetProgressResponse", new SetProgressResponse() {
            SetId = Guid.NewGuid().ToString(),
            Score = 0.6,
            IsCompleted = true,
            Targets = new[] { targetProgress },
        }, namingStrategy));

        return this;
    }
}
