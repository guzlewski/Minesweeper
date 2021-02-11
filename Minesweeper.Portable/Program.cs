using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Minesweeper.Server;
using Minesweeper.Server.Configuration;
using Minesweeper.Server.Data;
using Minesweeper.Server.Implementations;
using Minesweeper.Server.Interfaces;
using Minesweeper.Server.Logic;
using Minesweeper.Server.Mappers;

namespace Minesweeper.Portable
{
    class Program
    {
        static async Task Main()
        {
            await Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", false, true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<TcpServerSettings>(hostContext.Configuration.GetSection("TcpServer"));
                    services.Configure<List<Gamemode>>(hostContext.Configuration.GetSection("Gamemodes"));
                    services.Configure<LiteDBSettings>(hostContext.Configuration.GetSection("LiteDB"));
                    services.AddScoped<IMessageHandler, MessageHandler>();
                    services.AddScoped<IServer, TcpServer>();
                    services.AddSingleton(new CancellationTokenSource());
                    services.AddSingleton(AutoMapperConfig.Initialize());
                    services.AddSingleton<Random, ThreadSafeRandom>();
                    services.AddSingleton<DatabaseService>();
                    services.AddHostedService<ServerService>();
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                })
                .Build().RunAsync();
        }
    }

    class ServerService : IHostedService
    {
        private readonly IServer _server;

        public ServerService(IServer server)
        {
            _server = server;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _server.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _server.Stop();
            return Task.CompletedTask;
        }
    }
}
