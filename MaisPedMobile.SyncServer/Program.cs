using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MaisPedMobile.SyncServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:6580")
                .UseKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = null;
                })
                .UseStartup<Startup>();
    }
}
