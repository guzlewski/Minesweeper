using System.Collections.Generic;
using MessagePack;

namespace Minesweeper.Common.DTO
{
    [MessagePackObject]
    public class BoardDto
    {
        public const int MaxWidth = 50;
        public const int MaxHeight = 50;
        public const int MaxBombs = (MaxWidth - 1) * (MaxHeight - 1);

        [Key(0)]
        public int Width { get; set; }

        [Key(1)]
        public int Height { get; set; }

        [Key(2)]
        public int Bombs { get; set; }

        [Key(3)]
        public ICollection<FieldDto> Fields { get; set; }
    }
}
