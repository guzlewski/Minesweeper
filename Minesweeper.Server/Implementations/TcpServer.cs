using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minesweeper.Common.Extensions;
using Minesweeper.Common.Requests;
using Minesweeper.Server.Configuration;
using Minesweeper.Server.Interfaces;

namespace Minesweeper.Server
{
    public class TcpServer : IServer
    {
        private readonly TcpServerSettings _serverConfiguration;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger<TcpServer> _logger;
        private readonly IServiceProvider _provider;
        private readonly TcpListener _tcpListener;
        private readonly Thread _serverThread;

        public TcpServer(IOptionsSnapshot<TcpServerSettings> options, CancellationTokenSource cancellationTokenSource, ILogger<TcpServer> logger, IServiceProvider provider)
        {
            _serverConfiguration = options.Value;
            _cancellationTokenSource = cancellationTokenSource;
            _logger = logger;
            _provider = provider;
            _tcpListener = new TcpListener(IPAddress.Parse(_serverConfiguration.IPAddress), _serverConfiguration.Port);
            _serverThread = new Thread(async () => await ServerLoop());
        }

        public void Start()
        {
            _tcpListener.Start();
            _serverThread.Start();
            _logger.LogInformation("Server started at {0}", _tcpListener.LocalEndpoint);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _tcpListener.Stop();
            _cancellationTokenSource.Dispose();
            _serverThread.Join();
            _logger.LogInformation("Server stopped");
        }

        private async Task ServerLoop()
        {
            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var client = await _tcpListener.AcceptTcpClientAsync().ConfigureAwait(false);
                _ = HandleClient(client);
            }
        }

        private async Task HandleClient(TcpClient tcpClient)
        {
            NetworkStream stream = null;
            EndPoint clientEndPoint = null;

            try
            {
                clientEndPoint = tcpClient.Client.RemoteEndPoint;
                stream = tcpClient.GetStream();
                _logger.LogInformation("Client connected {0}", clientEndPoint);

                using var scope = _provider.CreateScope();
                var messageHandler = scope.ServiceProvider.GetRequiredService<IMessageHandler>();

                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    var message = await stream.ReceiveAsync<Request>(_cancellationTokenSource.Token).ConfigureAwait(false);
                    var response = messageHandler.GetResponse(message);
                    await stream.SendAsync(response, _cancellationTokenSource.Token).ConfigureAwait(false);
                }
            }
            catch
            {

            }
            finally
            {
                stream?.Close();
                tcpClient?.Close();
                _logger.LogInformation("Client disconnected {0}", clientEndPoint);
            }
        }
    }
}
