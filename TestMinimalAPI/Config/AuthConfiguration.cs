using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace TestMinimalAPI.Config;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAuth(this IServiceCollection collection, AppSettings settings)
    {
        collection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            var tokenParams = new TokenValidationParameters { NameClaimType = ClaimTypes.NameIdentifier };
            
            opt.Authority = $"https://{settings.OAuth.Domain}/";
            opt.Audience = settings.OAuth.Audience;
            opt.TokenValidationParameters = tokenParams;
        });
        
        collection.AddAuthorization(opt => opt.AddPolicy("TestUser", policy => policy.RequireClaim("https://testminimalapi.example.com/roles", "Test")));
        
        return collection;
    }
}