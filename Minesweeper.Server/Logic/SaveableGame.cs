using System;
using System.Collections.Generic;
using System.Text;
using Minesweeper.Common.Enums;
using Minesweeper.Server.Data;

namespace Minesweeper.Server.Logic
{
    public class SaveableGame : Game
    {
        private readonly DatabaseService _database;
        private readonly string _nickname;

        public SaveableGame(Random random, Gamemode gamemode, DatabaseService database, string nickname) : base(random, gamemode)
        {
            _database = database;
            _nickname = nickname;
        }

        public override void Play(int row, int column, FieldAction action)
        {
            var state = GameState;
            base.Play(row, column, action);

            if (state != GameState.Won && GameState == GameState.Won)
            {
                _database.Add(Gamemode, _nickname, RoundTime, StopTime);
            }
        }
    }
}
