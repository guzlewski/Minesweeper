using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Minesweeper.Client.Logic;
using Minesweeper.Client.Logic.Implementations;
using Minesweeper.Client.Logic.Interfaces;
using Minesweeper.Client.Logic.Settings;
using Minesweeper.Client.UI.Windows;
using Minesweeper.Common.DTO;

namespace Minesweeper.Client.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("appsettings-client.json", false, true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<ServerSettings>();
                    services.AddTransient<GameSettings>();
                    services.AddScoped<GameScreen>();
                    services.AddSingleton<ICommunicationHelper, TcpHelper>();
                    services.AddSingleton<IAssets, Assets>();
                    services.AddSingleton(new Gamemodes() { List = new List<GamemodeDto>() });
                    services.AddSingleton<SaveConnectionSettings>();
                    services.Configure<LastConnectionSettings>(hostContext.Configuration.GetSection("LastConnection"));
                })
                .Build();

            if (e.Args.Contains("manager"))
            {
                new ServerManager().Show();
            }
            else
            {
                host.Services.GetRequiredService<ServerSettings>().Show();
            }
        }
    }
}
