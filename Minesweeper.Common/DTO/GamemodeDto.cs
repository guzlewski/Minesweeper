using MessagePack;

namespace Minesweeper.Common.DTO
{
    [MessagePackObject]
    public class GamemodeDto
    {
        [Key(0)]
        public string Name { get; set; }

        [Key(1)]
        public int Width { get; set; }

        [Key(2)]
        public int Height { get; set; }

        [Key(3)]
        public int Bombs { get; set; }
    }
}
