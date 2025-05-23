using Microsoft.Extensions.FileProviders;

namespace TestMinimalAPI.Config;

public static class SpaConfiguration
{
    public static WebApplication ConfigureSpa(this WebApplication app)
    {

        app.UseDeveloperExceptionPage()
            .UseStaticFiles()
            .UseRouting();
        
        var staticFilesPath = Path.Combine(app.Environment.ContentRootPath, "ClientApp", "dist");

        app.UseDefaultFiles(new DefaultFilesOptions
        {
            FileProvider = new PhysicalFileProvider(staticFilesPath),
            RequestPath = ""
        });

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(staticFilesPath),
            RequestPath = ""
        });
        
        return app;
    }
}