using System;
using MessagePack;
using Minesweeper.Common.Enums;

namespace Minesweeper.Common.DTO
{
    [MessagePackObject]
    public class GameDto
    {
        [Key(0)]
        public GameState GameState { get; set; }

        [Key(1)]
        public BoardDto Board { get; set; }

        [Key(2)]
        public TimeSpan RoundTime { get; set; }
    }
}
