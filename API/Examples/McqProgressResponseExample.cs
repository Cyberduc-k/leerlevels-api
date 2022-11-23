using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class McqProgressResponseExample : OpenApiExample<McqProgressResponse>
{
    public override IOpenApiExample<McqProgressResponse> Build(NamingStrategy namingStrategy)
    {
        AnswerOption answer = new(Guid.NewGuid().ToString(), 0, "Example answer", true);

        Examples.Add(OpenApiExampleResolver.Resolve("McqProgresResponse_Sure", new McqProgressResponse() {
            McqId = Guid.NewGuid().ToString(),
            Answers = new GivenAnswerOption[] {
                new(answer, AnswerKind.Sure),
            },
        }, namingStrategy));

        Examples.Add(OpenApiExampleResolver.Resolve("McqProgresResponse_NotSure", new McqProgressResponse() {
            McqId = Guid.NewGuid().ToString(),
            Answers = new GivenAnswerOption[] {
                new(answer, AnswerKind.NotSure),
            },
        }, namingStrategy));

        Examples.Add(OpenApiExampleResolver.Resolve("McqProgresResponse_Guess", new McqProgressResponse() {
            McqId = Guid.NewGuid().ToString(),
            Answers = new GivenAnswerOption[] {
                new(answer, AnswerKind.Guess),
            },
        }, namingStrategy));

        return this;
    }
}
