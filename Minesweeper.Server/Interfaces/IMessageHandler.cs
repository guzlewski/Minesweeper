using Minesweeper.Common.Requests;

namespace Minesweeper.Server.Interfaces
{
    public interface IMessageHandler
    {
        public object GetResponse(Request request);
    }
}
