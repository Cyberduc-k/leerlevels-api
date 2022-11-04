using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model.DTO;
using Newtonsoft.Json.Serialization;

namespace API.Examples;

public class McqProgressDTOExample : OpenApiExample<McqProgressDTO>
{
    public override IOpenApiExample<McqProgressDTO> Build(NamingStrategy namingStrategy)
    {
        Examples.Add(OpenApiExampleResolver.Resolve("McqProgressDTO_Sure", new McqProgressDTO() {
            AnswerOptionId = Guid.NewGuid().ToString(),
            AnswerKind = Model.AnswerKind.Sure,
        }, namingStrategy));

        Examples.Add(OpenApiExampleResolver.Resolve("McqProgressDTO_NotSure", new McqProgressDTO() {
            AnswerOptionId = Guid.NewGuid().ToString(),
            AnswerKind = Model.AnswerKind.NotSure,
        }, namingStrategy));

        Examples.Add(OpenApiExampleResolver.Resolve("McqProgressDTO_Guess", new McqProgressDTO() {
            AnswerOptionId = Guid.NewGuid().ToString(),
            AnswerKind = Model.AnswerKind.Guess,
        }, namingStrategy));

        return this;
    }
}
