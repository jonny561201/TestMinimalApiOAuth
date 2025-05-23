using TestMinimalAPI.Config;
using TestMinimalAPI.Controllers;
using TestMinimalAPI.Middleware;
using TestMinimalAPI.Services.Services;

var builder = WebApplication.CreateBuilder(args);
var settings = builder.Configuration.Get<AppSettings>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ITestService, TestService>();
builder.Services.ConfigureAuth(settings);
builder.Services.ConfigureCors();

var app = builder.Build();

if (settings.Environment is "dev" or "local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.ConfigSpa();
app.UseMiddleware<GlobalExceptionHandler>();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.RegisterEndpoints();

app.Run();