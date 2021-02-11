using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Common.Extensions;

namespace Minesweeper.Tests
{
    [TestClass]
    public class NetworkStreamTests
    {
        [TestMethod]
        public async Task Read()
        {
            var server = new TcpListener(IPAddress.Parse("127.0.0.1"), 40000);
            server.Start();

            var client = new TcpClient();
            client.Connect("127.0.0.1", 40000);

            var serverStream = server.AcceptTcpClient().GetStream();
            var clientStream = client.GetStream();

            var send = "test message";
            await clientStream.SendAsync(send);
            var recieve = await serverStream.ReceiveAsync<string>();

            Assert.IsTrue(send == recieve);
        }
    }
}
