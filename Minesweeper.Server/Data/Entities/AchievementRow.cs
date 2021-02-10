using System;
using LiteDB;

namespace Minesweeper.Server.Data.Entities
{
    class AchievementRow
    {
        [BsonId]
        public int AchievementId { get; set; }
        public string Player { get; set; }
        public TimeSpan Time { get; set; }
        public DateTimeOffset Date { get; set; }
        public int GamemodeId { get; set; }
    }
}
