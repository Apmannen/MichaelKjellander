using MichaelKjellander.Components;
using MichaelKjellander.Config;
using MichaelKjellander.Services;
using MichaelKjellander.SharedUtils;
using Microsoft.Extensions.Options;

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

        InitializeDatabase(app.ApplicationServices).Wait();
    }

    private async Task InitializeDatabase(IServiceProvider serviceProvider)
    {
        AppConfig appConfig = serviceProvider.GetRequiredService<IOptions<AppConfig>>().Value;


        //serviceProvider.

        //using var scope = serviceProvider.CreateScope();


        /*using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BlogDataContext>();
        await context.CheckFillData();*/
    }
}