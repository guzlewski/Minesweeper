using MessagePack;

namespace Minesweeper.Common.Requests
{
    [MessagePackObject]
    public class Handshake : Request
    {
        [Key(0)]
        public string Nickname { get; set; }
    }
}
