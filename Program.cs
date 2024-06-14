using MichaelKjellander.Config;

namespace MichaelKjellander;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        AppEnvironment appEnvironment = EnvironmentUtil.GetAppEnvironment();

        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                if (AppConfig.IsAnyWww(appEnvironment))
                {
                    webBuilder.UseKestrel(kestrel =>
                    {
                        kestrel.ListenAnyIP(80);
                        kestrel.ListenAnyIP(443,
                            listenOptions =>
                            {
                                listenOptions.UseHttps(connectionOptions =>
                                {
                                    connectionOptions.UseLettuceEncrypt(kestrel.ApplicationServices);
                                });
                            });
                    });
                }

                webBuilder.UseStartup<Startup>();
            });
    }
}