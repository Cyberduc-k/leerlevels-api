using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace API.Configuration;
class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
{
    public override OpenApiInfo Info { get; set; } = new OpenApiInfo {
        Version = "1.1.0",
        Title = "LeerLevels API Specification",
        Description = "<h3>Description</h3> This contains the API models, object schemas and endpoint specifications of the LeerLevels platform Application. <br><br> This platform allows users to go through (sets of) educational targets which can be included into groups that a user can be a part of. These targets consist of explanations, a series of multiple choice questions and the possibility to provide feedback and store a user’s progress. A student user can also request to have a chat session with a teacher or coach type user and send messages between them to allow for more personalized interaction. <br><br> <h3>Authorization</h3> In order to start sending requests, you first have to enter a valid username & password combination in a request to the Tokens /authenticate endpoint. The received token can be entered into the authorization by entering: \"bearer [token]\"  into the value field and clicking the authorize button.",
        License = new OpenApiLicense {
            Name = "MIT",
            Url = new Uri("http://opensource.org/licenses/MIT"),
        }
    };
    public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
}
