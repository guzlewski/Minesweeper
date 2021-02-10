using System.Collections.Generic;
using MessagePack;

namespace Minesweeper.Common.DTO
{
    [MessagePackObject]
    public class RankingDto
    {
        [Key(0)]
        public GamemodeDto Gamemode { get; set; }

        [Key(1)]
        public IEnumerable<AchievementDto> Achievements { get; set; }

        [Key(2)]
        public int Total { get; set; }
    }
}
