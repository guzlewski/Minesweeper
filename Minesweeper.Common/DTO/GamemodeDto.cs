using MessagePack;

namespace Minesweeper.Common.DTO
{
    [MessagePackObject]
    public class GamemodeDto
    {
        public const int MinWidth = 6;
        public const int MaxWidth = 30;

        public const int MinHeight = 6;
        public const int MaxHeight = 30;

        public const int MinBombs = 1;
        public const int MaxBombs = (MaxWidth - 1) * (MaxHeight - 1);

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
