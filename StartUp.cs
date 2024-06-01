using MichaelKjellander.Components;
using MichaelKjellander.SharedUtils;

namespace MichaelKjellander;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        AppEnvironment environment =
            AppConfig.ParseAppEnvironment(Environment.GetEnvironmentVariable("SG_APPENVIRONMENT")!);

        if (AppConfig.IsAnyWww(environment))
        {
            services.AddLettuceEncrypt();
        }
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddRazorComponents().AddInteractiveServerComponents();

        services.AddHttpClient();

        services.Configure<AppConfig>(config =>
        {
            config.AppEnvironment = environment;
            config.ParseAndSetSiteUrl(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")!);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDefaultFiles();
        app.UseStaticFiles();

        //if (app.Environment.IsDevelopment())
        //{
            //app.UseSwagger();
            //app.UseSwaggerUI();
        //}

        app.UseHttpsRedirection();
        
        app.UseRouting();
        app.UseAuthorization();
        app.UseAntiforgery();
        app.UseResponseCaching();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("/index.html");
            
            endpoints.MapRazorComponents<App>().AddInteractiveServerRenderMode();
        });
    }
}