using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;
public class McqResponseExample : OpenApiExample<McqResponse>
{
    public override IOpenApiExample<McqResponse> Build(NamingStrategy namingStrategy)
    {
        List<AnswerOption> AnswerOptions = new List<AnswerOption>() { new AnswerOption() { Id = "1", Index = 2, IsCorrect = true, Text = "option1" } };
        
        Examples.Add(OpenApiExampleResolver.Resolve("Jan", new McqResponse() { Id = "f897655d0-56d5-42dd-8c12-4ecd66531f8", TargetId = "1", QuestionText = "Which programming language is better? ", Explanation = "C sharp is better than java", AllowRandom = true, AnswerOptions = AnswerOptions }, namingStrategy));

        return this;
    }
}
