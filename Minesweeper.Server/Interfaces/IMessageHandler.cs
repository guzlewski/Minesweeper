using Minesweeper.Common.Messages;

namespace Minesweeper.Server.Interfaces
{
    public interface IMessageHandler
    {
        public Message GetResponse(Message message);
    }
}
