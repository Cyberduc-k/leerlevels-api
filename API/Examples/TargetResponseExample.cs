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
public class TargetResponseExample : OpenApiExample<TargetResponse>
{
    public override IOpenApiExample<TargetResponse> Build(NamingStrategy namingStrategy)
    {
        McqResponse mcqResponse = new McqResponse() {
            AllowRandom = true,
            Id = "1",
            TargetId = "1",
            Explanation = "test explanation",
            QuestionText = "what is a Deconstructor?",
            AnswerOptions = new List<AnswerOption>() { new AnswerOption() { Id = "1", Index = 2, IsCorrect = true, Text = "option1" } }
        }; 

        List<McqResponse> mcqResponses = new List<McqResponse>();
        mcqResponses.Add(mcqResponse);
        Examples.Add(OpenApiExampleResolver.Resolve("Jan", new TargetResponse() { Id = "f897655d0-56d5-42dd-8c12-4ecd66531f8", Mcqs = mcqResponses, Description = "this is a test target", ImageUrl ="C:User/Desktop/photo.png", Label = "test label" , TargetExplanation = "test explanation", YoutubeId ="DQWRNQWE"}, namingStrategy));

        return this;
    }
}
