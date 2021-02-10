using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Windows;

namespace Minesweeper.Client.UI.Windows
{
    /// <summary>
    /// Interaction logic for ServerManager.xaml
    /// </summary>
    public partial class ServerManager : Window
    {
        private const string serviceName = "MinesweeperService";
        private readonly ServiceController serverController;
        private ServiceControllerStatus? serverStatus;

        public ServerManager(Window owner = null)
        {
            if (owner != null)
            {
                Owner = owner;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            InitializeComponent();

            serverController = new ServiceController(serviceName);
            UpdateServerStatus();
        }

        private void UpdateServerStatus()
        {
            try
            {
                serverStatus = serverController.Status;
            }
            catch (InvalidOperationException)
            {
                serverStatus = null;
            }

            if (serverStatus == null)
            {
                StatusTextBlock.Text = "Not Installed.";
            }
            else
            {
                StatusTextBlock.Text = serverStatus.ToString();

                if (serverStatus == ServiceControllerStatus.Stopped)
                {
                    ServerStartButton.IsEnabled = true;
                    ServerStopButton.IsEnabled = false;
                }
                else if (serverStatus == ServiceControllerStatus.Running)
                {
                    ServerStartButton.IsEnabled = false;
                    ServerStopButton.IsEnabled = true;
                }
            }
        }

        private void ServerStartButton_Click(object sender, RoutedEventArgs e)
        {
            serverController.Start();
            serverController.WaitForStatus(ServiceControllerStatus.Running);
            UpdateServerStatus();
        }

        private void ServerStopButton_Click(object sender, RoutedEventArgs e)
        {
            serverController.Stop();
            serverController.WaitForStatus(ServiceControllerStatus.Stopped);
            UpdateServerStatus();
        }
    }
}
