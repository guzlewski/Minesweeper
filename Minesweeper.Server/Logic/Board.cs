using System.Collections.Generic;

namespace Minesweeper.Server.Logic
{
    public class Board
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Bombs { get; set; }
        public List<Field> Fields { get; set; }
    }
}
