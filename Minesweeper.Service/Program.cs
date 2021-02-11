using System;
using System.Collections.Generic;
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
using Minesweeper.Server.Data;
using Minesweeper.Server.Implementations;
using Minesweeper.Server.Interfaces;
using Minesweeper.Server.Logic;
using Minesweeper.Server.Mappers;

namespace Minesweeper.Service
{
    static class Program
    {
        static void Main()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings-service.json", false, true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<TcpServerSettings>(hostContext.Configuration.GetSection("TcpServer"));
                    services.Configure<List<Gamemode>>(hostContext.Configuration.GetSection("Gamemodes"));
                    services.Configure<EventLogSettings>(hostContext.Configuration.GetSection("Logging:EventLog"));
                    services.Configure<LiteDBSettings>(hostContext.Configuration.GetSection("LiteDB"));
                    services.AddScoped<IMessageHandler, MessageHandler>();
                    services.AddScoped<IServer, TcpServer>();
                    services.AddScoped<MinesweeperService>();
                    services.AddSingleton(new CancellationTokenSource());
                    services.AddSingleton(AutoMapperConfig.Initialize());
                    services.AddSingleton<Random, ThreadSafeRandom>();
                    services.AddSingleton<DatabaseService>();
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.AddConfiguration(hostContext.Configuration.GetSection("Logging:EventLog"));
                    logging.AddEventLog();
                })
                .Build();

            ServiceBase.Run(host.Services.GetRequiredService<MinesweeperService>());
        }
    }
}
