using MessagePack;
using Minesweeper.Common.Enums;

namespace Minesweeper.Common.Requests
{
    [MessagePackObject]
    public class PlayGame : Request
    {
        [Key(0)]
        public int Row { get; set; }

        [Key(1)]
        public int Column { get; set; }

        [Key(2)]
        public FieldAction Action { get; set; }
    }
}
