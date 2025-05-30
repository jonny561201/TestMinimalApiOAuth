using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TestMinimalAPI.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAuth(this IServiceCollection service, AppSettings settings)
    {
        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            var tokenParams = new TokenValidationParameters
            {
                NameClaimType = ClaimTypes.NameIdentifier,
                ValidateIssuer = true,
                ValidIssuer = settings.OAuth.Issuer,
                ValidateLifetime = true,
                RoleClaimType = AuthClaims.Roles,
                ValidateAudience = true,
                ValidAudience = settings.OAuth.Audience
            };
            
            opt.Authority = settings.OAuth.Issuer;
            opt.Audience = settings.OAuth.Audience;
            opt.TokenValidationParameters = tokenParams;
        });
        
        service.AddAuthorization(opt => opt.AddPolicy(AuthPolicies.TestUser, policy => policy.RequireRole(AuthRoles.Test)));
        
        return service;
    }
}