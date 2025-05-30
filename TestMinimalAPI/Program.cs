using TestMinimalAPI.Config;
using TestMinimalAPI.Controllers;
using TestMinimalAPI.Data;
using TestMinimalAPI.Data.Config;
using TestMinimalAPI.Middleware;
using TestMinimalAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var settings = builder.Configuration.Get<AppSettings>();

builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureAuth(settings);
builder.Services.ConfigureCors();
builder.Services.AddPersonDbContext();

var app = builder.Build();

if (settings.Environment is "dev" or "local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.ConfigureSpa();
app.UseMiddleware<GlobalExceptionHandler>();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.RegisterEndpoints();

app.Run();


public partial class Program();
//CQRS
//CSP Headers
//Auth0 on React