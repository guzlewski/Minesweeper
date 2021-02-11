using System.Net.Sockets;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Server;
using Minesweeper.Server.Configuration;
using Moq;

namespace Minesweeper.Tests
{
    [TestClass]
    public class TcpServerTests
    {
        [TestMethod]
        public void StartStop()
        {
            var tcpServerSettings = new TcpServerSettings()
            {
                IPAddress = "127.0.0.1",
                Port = 23000
            };
            var logger = Mock.Of<ILogger<TcpServer>>();
            var token = new CancellationTokenSource();
            var optionsMock = new Mock<IOptionsSnapshot<TcpServerSettings>>();
            optionsMock.Setup(m => m.Value).Returns(tcpServerSettings);
            var server = new TcpServer(optionsMock.Object, token, logger, null);
            var tcpClientConnect = new TcpClient();
            var tcpClientDisconnect = new TcpClient();

            server.Start();
            tcpClientConnect.Connect(tcpServerSettings.IPAddress, tcpServerSettings.Port);
            Assert.IsTrue(tcpClientConnect.Connected);

            server.Stop();
            Assert.ThrowsException<SocketException>(() => tcpClientDisconnect.Connect(tcpServerSettings.IPAddress, tcpServerSettings.Port));
            Assert.IsFalse(tcpClientDisconnect.Connected);
        }
    }
}
