using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Common.DTO;
using Minesweeper.Common.Enums;
using Minesweeper.Common.Extensions;
using Minesweeper.Common.Requests;

namespace Minesweeper.Tests
{
    [TestClass]
    public class SerializerTests
    {
        [TestMethod]
        public void CreateGameTest()
        {
            var createGame = new CreateGame
            {
                Gamemode = new GamemodeDto { Bombs = 10, Height = 99, Width = 99, Name = "Name" }
            };
            var bytes = createGame.Serialize();

            var obj = bytes.Deserialize<CreateGame>();

            Assert.IsTrue(obj.Gamemode.Bombs == createGame.Gamemode.Bombs);
            Assert.IsTrue(obj.Gamemode.Height == createGame.Gamemode.Height);
            Assert.IsTrue(obj.Gamemode.Name == createGame.Gamemode.Name);
            Assert.IsTrue(obj.Gamemode.Width == createGame.Gamemode.Width);
        }

        [TestMethod]
        public void GetRankingTest()
        {
            var getRanking = new GetRanking();
            var bytes = getRanking.Serialize();

            var obj = bytes.Deserialize<GetRanking>();
        }

        [TestMethod]
        public void HandshakeTest()
        {
            var handshake = new Handshake { Nickname = "Nickname Testowy" };
            var bytes = handshake.Serialize();

            var obj = bytes.Deserialize<Handshake>();

            Assert.IsTrue(obj.Nickname == handshake.Nickname);
        }

        [TestMethod]
        public void PlayGame()
        {
            var playGame = new PlayGame { Action = FieldAction.Flag, Column = 1, Row = 23 };
            var bytes = playGame.Serialize();

            var obj = bytes.Deserialize<PlayGame>();

            Assert.IsTrue(obj.Action == playGame.Action);
            Assert.IsTrue(obj.Column == playGame.Column);
            Assert.IsTrue(obj.Row == playGame.Row);
        }
    }
}
