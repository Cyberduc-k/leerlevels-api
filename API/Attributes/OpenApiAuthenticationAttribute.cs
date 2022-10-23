using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace API.Attributes;

public class OpenApiAuthenticationAttribute : OpenApiSecurityAttribute
{
    public OpenApiAuthenticationAttribute() : base("LeerLevelsAuthentication", SecuritySchemeType.Http)
    {
        Description = "JWT used for authorization";
        In = OpenApiSecurityLocationType.Header;
        Scheme = OpenApiSecuritySchemeType.Bearer;
        BearerFormat = "JWT";
    }
}
