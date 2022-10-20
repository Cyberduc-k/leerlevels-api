using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace API.Attributes;
public class OpenApiAuthentication : OpenApiSecurityAttribute
{
    public OpenApiAuthentication() : base("LeerLevelsAuthentication", SecuritySchemeType.Http)
    {
        Description = "JWT used for authorization";
        In = OpenApiSecurityLocationType.Header;
        Scheme = OpenApiSecuritySchemeType.Bearer;
        BearerFormat = "JWT";
    }
}
