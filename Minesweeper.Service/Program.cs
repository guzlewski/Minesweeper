using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using Minesweeper.Server;
using Minesweeper.Server.Configuration;
using Minesweeper.Server.Implementations;
using Minesweeper.Server.Interfaces;

namespace Minesweeper.Service
{
    static class Program
    {
        static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Minesweeper", "Service", "appsettings.json"), optional: false);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<TcpServerSettings>(hostContext.Configuration.GetSection("TcpServer"));
                    services.AddScoped<IMessageHandler, MessageHandler>();
                    services.AddScoped<IServer, TcpServer>();
                    services.AddScoped<MinesweeperService>();
                    services.AddSingleton(new CancellationTokenSource());
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                    logging.AddEventLog(hostContext.Configuration.GetSection("Logging:EventLog").Get<EventLogSettings>());
                })
                .Build();

            ServiceBase.Run(host.Services.GetRequiredService<MinesweeperService>());
        }
    }
}
