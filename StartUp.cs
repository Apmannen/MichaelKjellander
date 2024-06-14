using MichaelKjellander.Components;
using MichaelKjellander.Config;
using MichaelKjellander.Data;
using MichaelKjellander.Scripts.Startup;
using MichaelKjellander.Services;

namespace MichaelKjellander;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        AppEnvironment environment = EnvironmentUtil.GetAppEnvironment();

        if (AppConfig.IsAnyWww(environment))
        {
            services.AddLettuceEncrypt();
        }

        services.AddDbContext<BlogDataContext>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddRazorComponents().AddInteractiveServerComponents();

        services.AddScoped<JsService>();

        services.AddHttpClient();
        services.AddHttpClient<WpApiService>();

        services.Configure<AppConfig>(config => { EnvironmentUtil.SetupAppConfig(config); });
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

        StartupScript startupScript = EnvironmentUtil.GetStartupScript();
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
        await new CleanWpDbScript().Run(context, service);
    }
}