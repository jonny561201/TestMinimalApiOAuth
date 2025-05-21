using TestMinimalAPI.Config;
using TestMinimalAPI.Controllers;

var builder = WebApplication.CreateBuilder(args);
var settings = builder.Configuration.Get<AppSettings>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureAuth(settings);

var app = builder.Build();

if (settings.Environment == "dev" || settings.Environment == "local")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.RegisterEndpoints();

app.Run();
