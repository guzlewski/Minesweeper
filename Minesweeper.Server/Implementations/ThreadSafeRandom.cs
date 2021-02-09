using System;
using System.Security.Cryptography;
using System.Threading;

namespace Minesweeper.Server.Implementations
{
    public class ThreadSafeRandom : Random
    {
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();
        private readonly ThreadLocal<Random> _random = new ThreadLocal<Random>(() =>
        {
            var buffer = new byte[4];
            _generator.GetBytes(buffer);

            return new Random(BitConverter.ToInt32(buffer, 0));
        });

        public override int Next()
        {
            return _random.Value.Next();
        }
    }
}
