namespace TestMinimalAPI.Config;

public static class CorsConfiguration
{
    public static IServiceCollection ConfigureCors(this IServiceCollection service)
    {
        service.AddCors(corsOpt => corsOpt.AddPolicy("AllowAllOrigin", policy =>
            {
                policy.AllowAnyHeader()
                      .AllowAnyOrigin()
                      .AllowAnyMethod();
            }
        ));
        
        return service;
    }
}