using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Model;
public class TokenIdentityValidationParameters : TokenValidationParameters
{
    public TokenIdentityValidationParameters(string Issuer, string Audience, SymmetricSecurityKey SecurityKey)
    {
        RequireSignedTokens = true;
        ValidAudience = Audience;
        ValidateAudience = true;
        ValidIssuer = Issuer;
        ValidateIssuer = true;
        ValidateIssuerSigningKey = true;
        ValidateLifetime = true;
        IssuerSigningKey = SecurityKey;
        AuthenticationType = "Bearer";
    }
}
