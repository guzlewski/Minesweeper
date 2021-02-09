using Minesweeper.Common.Enums;

namespace Minesweeper.Server.Logic
{
    public class Field
    {
        public FieldState State { get; set; }
        public int Value { get; set; }
        public bool IsBomb { get; set; }
    }
}
