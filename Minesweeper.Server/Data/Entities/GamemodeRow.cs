using LiteDB;

namespace Minesweeper.Server.Data.Entities
{
    class GamemodeRow
    {
        [BsonId]
        public int GamemodeId { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Bombs { get; set; }
    }
}
