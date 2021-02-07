using System.ServiceProcess;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Minesweeper.Server;
using Minesweeper.Server.Configuration;
using Minesweeper.Server.Interfaces;

namespace Minesweeper.Service
{
    static class Program
    {
        static void Main()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<ServerConfiguration>(hostContext.Configuration.GetSection("ServerSettings"));
                    services.AddScoped<IMessageHandler, MessageHandler>();
                    services.AddScoped<IServer, TcpServer>();
                    services.AddScoped<MinesweeperServerService>();
                    services.AddSingleton(new CancellationTokenSource());
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddEventLog(eventLogSettings =>
                    {
                        eventLogSettings.LogName = "Minesweeper Server Logs";
                        eventLogSettings.SourceName = "Minesweeper.Service";
                    });
                })
                .Build();

            ServiceBase.Run(builder.Services.GetRequiredService<MinesweeperServerService>());
        }
    }
}
