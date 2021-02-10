using System;
using MessagePack;

namespace Minesweeper.Common.DTO
{
    [MessagePackObject]
    public class AchievementDto
    {
        [Key(0)]
        public string Player { get; set; }

        [Key(1)]
        public TimeSpan Time { get; set; }

        [Key(2)]
        public DateTimeOffset Date { get; set; }
    }
}
