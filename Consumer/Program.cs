using System;
using Messages;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Consumer
{
    class Program
    {
        public static IHostBuilder CreateHostBuilder()
        {
            var hostBuilder = Host.CreateDefaultBuilder();
            hostBuilder.ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Receiver>();
                });

            return hostBuilder;
        }

        static void Main()
        {
            CreateHostBuilder().Build().Run();

            Console.ReadLine();
        }
    }
}