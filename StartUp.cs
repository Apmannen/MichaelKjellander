using MichaelKjellander.Components;
using MichaelKjellander.SharedUtils;

namespace MichaelKjellander;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddRazorComponents().AddInteractiveServerComponents();

        services.AddHttpClient();

        services.Configure<AppConfig>(config =>
        {
            config.AppEnvironment = AppConfig.ParseAppEnvironment(Environment.GetEnvironmentVariable("SG_APPENVIRONMENT")!);
            config.SiteUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")!;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //Console.WriteLine("*** app="+app);
        
        app.UseDefaultFiles();
        app.UseStaticFiles();

// Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
            //app.UseSwagger();
            //app.UseSwaggerUI();
        //}

        app.UseHttpsRedirection();

        
        
        app.UseRouting();
        app.UseAuthorization();
        app.UseAntiforgery();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("/index.html");
            
            endpoints.MapRazorComponents<App>().AddInteractiveServerRenderMode();
        });

        //app.MapControllers();

        //app.MapFallbackToFile("/index.html");

        
        //app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

        //app.Run();
    }
}