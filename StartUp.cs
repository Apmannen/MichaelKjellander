using MichaelKjellander.Communicators;
using MichaelKjellander.Views.Components;
using MichaelKjellander.Config;
using MichaelKjellander.Data;
using MichaelKjellander.Scripts.Startup;
using MichaelKjellander.TmpName;

namespace MichaelKjellander;

public class Startup
{
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
        services.AddScoped<TranslationService>();

        services.AddHttpClient();
        services.AddHttpClient<WpApiCommunicator>();

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
        WpApiCommunicator communicator = new WpApiCommunicator(client);
        await new CleanWpDbScript().Run(context, communicator);
    }
}