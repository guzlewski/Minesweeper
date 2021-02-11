using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Common.DTO;
using Minesweeper.Common.Enums;

namespace Minesweeper.Tests
{
    [TestClass]
    public class DtosTests
    {
        [TestMethod]
        public void Achievement()
        {
            var player = "nick";
            var time = TimeSpan.FromSeconds(213);
            var date = DateTimeOffset.Now;

            var achievement = new AchievementDto() { Player = player, Time = time, Date = date };

            Assert.IsTrue(achievement.Player == player);
            Assert.IsTrue(achievement.Time == time);
            Assert.IsTrue(achievement.Date == date);
        }

        [TestMethod]
        public void Board()
        {
            var bombs = 10;
            var height = 31;
            var width = 12;
            var fields = new List<FieldDto>();

            var board = new BoardDto() { Bombs = bombs, Height = height, Width = width, Fields = fields };

            Assert.IsTrue(board.Bombs == bombs);
            Assert.IsTrue(board.Height == height);
            Assert.IsTrue(board.Width == width);
            Assert.IsTrue(board.Fields == fields);
        }

        [TestMethod]
        public void Field()
        {
            var state = FieldState.Flag;
            var value = 5;

            var board = new FieldDto() { State = FieldState.Flag, Value = value };

            Assert.IsTrue(board.State == state);
            Assert.IsTrue(board.Value == value);
        }

        [TestMethod]
        public void Game()
        {
            var board = new BoardDto();
            var round = TimeSpan.FromSeconds(20);
            var state = GameState.Won;

            var game = new GameDto() { Board = board, GameState = state, RoundTime = round };

            Assert.IsTrue(game.Board == board);
            Assert.IsTrue(game.RoundTime == round);
            Assert.IsTrue(game.GameState == state);
        }

        [TestMethod]
        public void Ranking()
        {
            var achievements = new List<AchievementDto>();
            var gamemode = new GamemodeDto();
            var total = 10;

            var game = new RankingDto() { Achievements = achievements, Gamemode = gamemode, Total = total };

            Assert.IsTrue(game.Achievements == achievements);
            Assert.IsTrue(game.Gamemode == gamemode);
            Assert.IsTrue(game.Total == total);
        }
    }
}
