using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minesweeper.Common;
using Minesweeper.Server.Configuration;
using Minesweeper.Server.Interfaces;

namespace Minesweeper.Server
{
    public class TcpServer : IServer
    {
        private readonly ServerConfiguration _serverConfiguration;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger<TcpServer> _logger;
        private readonly IMessageHandler _messageHandler;
        private readonly TcpListener _tcpListener;

        public TcpServer(IOptionsMonitor<ServerConfiguration> options, CancellationTokenSource cancellationTokenSource, ILogger<TcpServer> logger, IMessageHandler messageHandler)
        {
            _serverConfiguration = options.CurrentValue;
            _cancellationTokenSource = cancellationTokenSource;
            _logger = logger;
            _messageHandler = messageHandler;
            _tcpListener = new TcpListener(IPAddress.Parse(_serverConfiguration.IPAddress), _serverConfiguration.Port);
        }

        public void Start()
        {
            _tcpListener.Start();
            _logger.LogInformation("Server started at {0}", _tcpListener.LocalEndpoint);
            Task.Run(ServerLoop, _cancellationTokenSource.Token);
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
            _tcpListener.Stop();
            _cancellationTokenSource.Dispose();
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

            try
            {
                stream = tcpClient.GetStream();

                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    var message = await stream.ReceiveMessage(_cancellationTokenSource.Token).ConfigureAwait(false);
                    var response = _messageHandler.GetResponse(message);
                    await stream.SendMessage(response, _cancellationTokenSource.Token).ConfigureAwait(false);
                }
            }
            finally
            {
                stream?.Close();
                tcpClient?.Close();
            }
        }
    }
}
