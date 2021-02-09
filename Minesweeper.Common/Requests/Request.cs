using MessagePack;

namespace Minesweeper.Common.Requests
{
    [Union(0, typeof(CreateGame))]
    [Union(1, typeof(GetRanking))]
    [Union(2, typeof(Handshake))]
    [Union(3, typeof(PlayGame))]
    public abstract class Request
    {

    }
}
