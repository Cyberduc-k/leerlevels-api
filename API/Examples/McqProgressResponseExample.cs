using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class McqProgressResponseExample : OpenApiExample<McqProgressResponse>
{
    public override IOpenApiExample<McqProgressResponse> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("McqProgresResponse_Sure", new McqProgressResponse() {
            McqId = Guid.NewGuid().ToString(),
            AnswerOptionId = Guid.NewGuid().ToString(),
            AnswerKind = Model.AnswerKind.Sure,
            Score = 1.0,
        }, namingStrategy));

        Examples.Add(OpenApiExampleResolver.Resolve("McqProgresResponse_NotSure", new McqProgressResponse() {
            McqId = Guid.NewGuid().ToString(),
            AnswerOptionId = Guid.NewGuid().ToString(),
            AnswerKind = Model.AnswerKind.NotSure,
            Score = 0.6,
        }, namingStrategy));

        Examples.Add(OpenApiExampleResolver.Resolve("McqProgresResponse_Guess", new McqProgressResponse() {
            McqId = Guid.NewGuid().ToString(),
            AnswerOptionId = Guid.NewGuid().ToString(),
            AnswerKind = Model.AnswerKind.Guess,
            Score = 0.3,
        }, namingStrategy));

        return this;
    }
}
