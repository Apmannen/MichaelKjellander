
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
                    if (appEnvironment is AppEnvironment.Prod or AppEnvironment.WwwDev)
                    {
                        webBuilder.UseKestrel(kestrel =>
                        {
                            //kestrel.ListenAnyIP(80);
                            kestrel.ListenAnyIP(443, listenOptions =>
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
}
