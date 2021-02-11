using System.Collections.Generic;
using MessagePack;

namespace Minesweeper.Common.DTO
{
    [MessagePackObject]
    public class BoardDto
    {
        [Key(0)]
        public int Width { get; set; }

        [Key(1)]
        public int Height { get; set; }

        [Key(2)]
        public int Bombs { get; set; }

        [Key(3)]
        public List<FieldDto> Fields { get; set; }
    }
}
