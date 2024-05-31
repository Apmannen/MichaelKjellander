
using MichaelKjellander.SharedUtils;

namespace MichaelKjellander
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            AppEnvironment appEnvironment = AppConfig.ParseAppEnvironment(Environment.GetEnvironmentVariable("SG_APPENVIRONMENT")!);
            
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    if (appEnvironment == AppEnvironment.Prod)
                    {
                        webBuilder.UseKestrel(options =>
                        {
                            //options.ListenAnyIP(5000); // HTTP
                            options.ListenAnyIP(443, listenOptions =>
                            {
                                listenOptions.UseHttps("/etc/letsencrypt/live/new.michaelkjellander.se/fullchain.pem",
                                    "/etc/letsencrypt/live/new.michaelkjellander.se/privkey.pem");
                            });
                        });
                    }
                    webBuilder.UseStartup<Startup>();
                });
            
        }
            
    }
}

/*var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddHttpClient();

builder.Services.Configure<AppConfig>(config =>
{
    config.SetAppEnvironment(Environment.GetEnvironmentVariable("SG_APPENVIRONMENT")!);
    config.SiteUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")!;
});

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseKestrel(options =>
        {
            //options.ListenAnyIP(443); // HTTP
            options.ListenAnyIP(443, listenOptions =>
            {
                listenOptions.UseHttps("/etc/letsencrypt/live/new.michaelkjellander.se/fullchain.pem",
                    "/etc/letsencrypt/live/new.michaelkjellander.se/privkey.pem");
            });
        });
    });
hostBuilder.StartAsync();

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();*/

