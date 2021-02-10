using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minesweeper.Client.Logic;
using Minesweeper.Client.Logic.Interfaces;
using Minesweeper.Client.Logic.Settings;
using Minesweeper.Common.DTO;
using Minesweeper.Common.Requests;

namespace Minesweeper.Client.UI.Windows
{
    /// <summary>
    /// Interaction logic for ServerSettings.xaml
    /// </summary>
    public partial class ServerSettings : Window
    {
        private readonly ICommunicationHelper _communication;
        private readonly IServiceProvider _provider;
        private readonly LastConnectionSettings _settings;
        private readonly SaveConnectionSettings _saveConnection;

        public ServerSettings(ICommunicationHelper communication, IServiceProvider provider, IOptionsMonitor<LastConnectionSettings> options, SaveConnectionSettings saveConnection)
        {
            InitializeComponent();
            _communication = communication;
            _provider = provider;
            _settings = options.CurrentValue;
            _saveConnection = saveConnection;

            HostTextBox.Text = _settings.Host;
            PortTextBox.Text = _settings.Port;
            NickTextBox.Text = _settings.Nickname;
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(HostTextBox.Text))
            {
                StatusLabel.Content = "Invalid or empty hostname.";
                return;
            }

            if (string.IsNullOrWhiteSpace(PortTextBox.Text) || !int.TryParse(PortTextBox.Text, out int port))
            {
                StatusLabel.Content = "Invalid or empty port.";
                return;
            }

            if (string.IsNullOrWhiteSpace(NickTextBox.Text))
            {
                StatusLabel.Content = "Invalid or empty nickname.";
                return;
            }

            LockUI();
            StatusLabel.Content = "Connecting...";

            if (!await _communication.TryConnectAsync(HostTextBox.Text, port))
            {
                StatusLabel.Content = "Can't connect to server.";
                UnlockUI();
                return;
            }

            var response = await _communication.SendAndRecieveAsync<List<GamemodeDto>>(new Handshake { Nickname = NickTextBox.Text });

            if (response == null)
            {
                StatusLabel.Content = "Can't recieve response from server.";
                UnlockUI();
                return;
            }

            _saveConnection.Save(new LastConnectionSettings
            {
                Host = HostTextBox.Text,
                Port = PortTextBox.Text,
                Nickname = NickTextBox.Text
            });

            _provider.GetRequiredService<Gamemodes>().List = response;
            _provider.GetRequiredService<GameSettings>().Show();
            Close();
        }

        private void ServerManageButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsRunAsAdmin())
            {
                var manageServer = new ServerManager(this);
                manageServer.ShowDialog();
            }
            else
            {
                var proc = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = AppDomain.CurrentDomain.FriendlyName,
                    Verb = "runas",
                    Arguments = "manager"
                };

                try
                {
                    var process = Process.Start(proc);
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private bool IsRunAsAdmin()
        {
            var id = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void LockUI()
        {
            HostTextBox.IsEnabled = false;
            PortTextBox.IsEnabled = false;
            NickTextBox.IsEnabled = false;
            ConnectButton.IsEnabled = false;
            ServerManageButton.IsEnabled = false;
        }

        private void UnlockUI()
        {
            HostTextBox.IsEnabled = true;
            PortTextBox.IsEnabled = true;
            NickTextBox.IsEnabled = true;
            ConnectButton.IsEnabled = true;
            ServerManageButton.IsEnabled = true;
        }
    }
}
