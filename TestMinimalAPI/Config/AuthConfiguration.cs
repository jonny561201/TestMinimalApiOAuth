using System.Security.Claims;
using System.Text.Json;
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

            opt.Events = new JwtBearerEvents
            {
                OnTokenValidated = context => ParseCustomClaim(context)
            };
        });
        
        service.AddAuthorizationBuilder().AddPolicy(AuthPolicies.TestUser, policy => policy.RequireRole(AuthRoles.Test));
        
        return service;
    }

    private static Task ParseCustomClaim(TokenValidatedContext context)
    {
        var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
        var customClaim = claimsIdentity?.FindFirst("custom-claim");

        if (customClaim == null) return Task.CompletedTask;
        
        var area = JsonSerializer.Deserialize<CustomClaim>(customClaim.Value)?.Area;
        
        foreach (var claim in area ?? [])
        {
            claimsIdentity?.AddClaim(new Claim("area", claim));
        }

        return Task.CompletedTask;
    }
}



public record CustomClaim
{
    public List<string> Area { get; init; }
}