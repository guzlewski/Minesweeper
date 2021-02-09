using MessagePack;
using Minesweeper.Common.Enums;

namespace Minesweeper.Common.DTO
{
    [MessagePackObject]
    public class FieldDto
    {
        [Key(0)]
        public FieldState State { get; set; }

        [Key(1)]
        public int Value { get; set; }
    }
}
