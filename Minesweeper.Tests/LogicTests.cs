using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minesweeper.Common.Enums;
using Minesweeper.Server.Logic;

namespace Minesweeper.Tests
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void Board()
        {
            var bombs = 9;
            var width = 10;
            var height = 11;
            var fields = new List<Field>();

            var board = new Board { Bombs = bombs, Fields = fields, Height = height, Width = width };

            Assert.IsTrue(board.Bombs == bombs);
            Assert.IsTrue(board.Width == width);
            Assert.IsTrue(board.Height == height);
            Assert.IsTrue(board.Fields == fields);
        }

        [TestMethod]
        public void Field()
        {
            var bomb = false;
            var state = FieldState.Mark;
            var value = 13;

            var field = new Field { IsBomb = bomb, State = state, Value = value };

            Assert.IsTrue(field.IsBomb == bomb);
            Assert.IsTrue(field.State == state);
            Assert.IsTrue(field.Value == value);
        }

        [TestMethod]
        public void Game()
        {
            var random = new Random();
            var gamemode = new Gamemode { Bombs = 10, Height = 9, Width = 9, Name = "Easy" };

            var game = new Game(random, gamemode);

            Assert.IsTrue(game.Gamemode == gamemode);
            Assert.IsTrue(game.GameState == GameState.New);

            game.Play(0, 0, FieldAction.Open);

            Assert.IsTrue(game.Board.Fields[0].State != FieldState.Close);
        }

        [TestMethod]
        public void Gamemode()
        {
            var bombs = 10;
            var height = 9;
            var width = 9;
            var name = "Easy";

            var gamemode = new Gamemode { Bombs = bombs, Height = height, Width = width, Name = name };

            Assert.IsTrue(gamemode.Bombs == bombs);
            Assert.IsTrue(gamemode.Height == height);
            Assert.IsTrue(gamemode.Width == width);
            Assert.IsTrue(gamemode.Name == name);
        }
    }
}
