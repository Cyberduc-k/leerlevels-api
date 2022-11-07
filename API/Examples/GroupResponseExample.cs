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
public class GroupResponseExample : OpenApiExample<GroupResponse>
{
    public override IOpenApiExample<GroupResponse> Build(NamingStrategy namingStrategy)
    {
        UserResponse userResponse = new UserResponse() {
            Id = "a75e3fe7-f519-48de-a106-79f788a1b479",
            Email = "jan@gmail.com",
            FirstName = "Jan",
            LastName = "Groothuis",
            UserName = "JanG#1",
            Role = UserRole.Student,
            ShareCode = "DTRY-WQER-PIGU-VNSA"
        };

        List<UserResponse> usersresponses = new List<UserResponse>();
        usersresponses.Add(userResponse);

        SetResponse setResponse = new SetResponse() {
            Id = "1",
            Targets = new List<TargetResponse>()
        };

        List<SetResponse> setResponses = new List<SetResponse>();   
        setResponses.Add(setResponse);

        Examples.Add(OpenApiExampleResolver.Resolve("Jan", new GroupResponse() { Id = "f68755d0-56d5-42dd-8c12-4ecd72b531f8", Name = "inholland", Subject = "IT", EducationType = EducationType.Havo, SchoolYear = SchoolYear.One, Users = usersresponses, Sets = setResponses }, namingStrategy));

        return this;
    }
}