using MessagePack;
using Minesweeper.Common.DTO;

namespace Minesweeper.Common.Requests
{
    [MessagePackObject]
    public class CreateGame : Request
    {
        [Key(0)]
        public GamemodeDto Gamemode { get; set; }
    }
}
