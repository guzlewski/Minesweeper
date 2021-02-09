using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Minesweeper.Common.Extensions
{
    public static class NetworkStreamExtensions
    {
        private const int headerSize = sizeof(int);

        public static async Task SendAsync<T>(this NetworkStream stream, T message, CancellationToken cancellationToken = default)
        {
            var body = message.Serialize();
            var header = BitConverter.GetBytes(body.Length);

            await stream.WriteAsync(header, 0, headerSize, cancellationToken).ConfigureAwait(false);
            await stream.WriteAsync(body, 0, body.Length, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<T> ReceiveAsync<T>(this NetworkStream stream, CancellationToken cancellationToken = default)
        {
            var header = await stream.ReadExactlyNBytesAsync(headerSize, cancellationToken).ConfigureAwait(false);
            var bodySize = BitConverter.ToInt32(header, 0);
            var body = await stream.ReadExactlyNBytesAsync(bodySize, cancellationToken).ConfigureAwait(false);

            return body.Deserialize<T>();
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
