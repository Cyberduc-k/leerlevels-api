using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Model;
using Model.Response;
using Newtonsoft.Json.Serialization;

namespace API.Examples;
public class GroupResponseExample : OpenApiExample<GroupResponse>
{
    public override IOpenApiExample<GroupResponse> Build(NamingStrategy namingStrategy)
    {
        UserResponse userResponse = new() {
            Id = "a75e3fe7-f519-48de-a106-79f788a1b479",
            Email = "jan@gmail.com",
            FirstName = "Jan",
            LastName = "Groothuis",
            UserName = "JanG#1",
            Role = UserRole.Student,
            ShareCode = "DTRY-WQER-PIGU-VNSA"
        };

        List<UserResponse> usersresponses = new();
        usersresponses.Add(userResponse);

        SetResponse setResponse = new() {
            Id = "1",
            Targets = new List<TargetResponse>()
        };

        List<SetResponse> setResponses = new();
        setResponses.Add(setResponse);

        Examples.Add(OpenApiExampleResolver.Resolve("Jan", new GroupResponse() { Id = "f68755d0-56d5-42dd-8c12-4ecd72b531f8", Name = "inholland", Subject = "IT", EducationType = EducationType.Havo, SchoolYear = SchoolYear.One, Users = usersresponses, Sets = setResponses }, namingStrategy));

        return this;
    }
}