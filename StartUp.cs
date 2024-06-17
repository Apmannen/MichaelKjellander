using MichaelKjellander.Views.Components;
using MichaelKjellander.Config;
using MichaelKjellander.Data;
using MichaelKjellander.Scripts.Startup;
using MichaelKjellander.Services;
using MichaelKjellander.Views.Services;
using MichaelKjellander.Views.Services.Api;

namespace MichaelKjellander;

public class Startup
{
    
    /// <summary>
    /// * Transient: A new instance of the service is created each time it is requested.
    /// * Scoped: A new instance of the service is created per request within a given scope (e.g., an HTTP request in a
    /// server application, or a user session in a Blazor WebAssembly application).
    /// * Singleton: A single instance of the service is created and reused throughout the application's lifetime.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        AppEnvironment environment = EnvVariables.GetAppEnvironment();

        if (AppConfig.IsAnyWww(environment))
        {
            services.AddLettuceEncrypt();
        }

        services.AddDbContext<BlogDataContext>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddRazorComponents().AddInteractiveServerComponents();

        services.AddScoped<JsService>();
        services.AddSingleton<TranslationsService>();

        services.AddHttpClient();
        services.AddHttpClient<WpApiService>();

        services.Configure<AppConfig>(config => { EnvVariables.SetupAppConfig(config); });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
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

        StartupScript startupScript = EnvVariables.GetStartupScript();
        switch (startupScript)
        {
            case StartupScript.CleanWpDb:
                CleanWpDb(app.ApplicationServices).Wait();
                System.Environment.Exit(0);
                break;
            default:
                break;
        }
    }

    private async Task CleanWpDb(IServiceProvider serviceProvider)
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        BlogDataContext context = scope.ServiceProvider.GetRequiredService<BlogDataContext>();
        using HttpClient client = new HttpClient();
        WpApiService service = new WpApiService(client);
        await new CleanWpDbScript(context, service).Run();
    }
}