using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using UserService;

namespace konnect_player_info
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            var HOST = Environment.GetEnvironmentVariable("HOST");
            var PORT = Environment.GetEnvironmentVariable("PORT");

            return WebHost.CreateDefaultBuilder(args)
                .UseUrls($"http://{HOST}:{PORT}")
                .UseKestrel()
                .UseStartup<Startup>();
        }
    }
}
