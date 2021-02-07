using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Minesweeper.Common.Messages;

namespace Minesweeper.Common
{
    public static class NetworkStreamExtensions
    {
        private const int headerSize = sizeof(int);

        public static async Task<Message> ReceiveMessage(this NetworkStream stream, CancellationToken cancellationToken = default)
        {
            var header = await stream.ReadExactlyNBytesAsync(headerSize, cancellationToken).ConfigureAwait(false);
            var bodySize = BitConverter.ToInt32(header, 0);
            var body = await stream.ReadExactlyNBytesAsync(bodySize, cancellationToken).ConfigureAwait(false);

            return Message.FromBytes(body);
        }

        public static async Task SendMessage(this NetworkStream stream, Message message, CancellationToken cancellationToken = default)
        {
            var messageBody = message.ToBytes();
            var messageHeader = BitConverter.GetBytes(messageBody.Length);

            await stream.WriteAsync(messageHeader, 0, messageHeader.Length, cancellationToken).ConfigureAwait(false);
            await stream.WriteAsync(messageBody, 0, messageBody.Length, cancellationToken).ConfigureAwait(false);
        }

        private static async Task<byte[]> ReadExactlyNBytesAsync(this NetworkStream stream, int n, CancellationToken cancellationToken)
        {
            if (n < 1)
            {
                throw new ArgumentException("Number of bytes to read must be greather than 0", nameof(n));
            }

            var bytes = new byte[n];
            var bytesRead = 0;

            while (bytesRead != n)
            {
                var currentBytes = await stream.ReadAsync(bytes, bytesRead, n - bytesRead, cancellationToken).ConfigureAwait(false);

                if (currentBytes == 0)
                {
                    throw new InvalidOperationException("Comunnication closed.");
                }

                bytesRead += currentBytes;
            }

            return bytes;
        }
    }
}
