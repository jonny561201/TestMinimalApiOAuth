using Microsoft.Extensions.FileProviders;

namespace TestMinimalAPI.Config;

public static class SpaConfiguration
{
    public static WebApplication ConfigureSpa(this WebApplication app)
    {
        var staticFilesPath = Path.Combine(app.Environment.ContentRootPath, "ClientApp", "dist");

        var defaultOptions = new DefaultFilesOptions
        {
            FileProvider = new PhysicalFileProvider(staticFilesPath),
            RequestPath = ""
        };
        var fileOptions = new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(staticFilesPath),
            RequestPath = ""
        };
        
        app.UseDeveloperExceptionPage()
            .UseRouting()
            .UseDefaultFiles(defaultOptions)
            .UseStaticFiles(fileOptions);
        
        return app;
    }
}