using System;
using System.Collections.Generic;
using System.Linq;
using Minesweeper.Common.Enums;
using Minesweeper.Server.Data;

namespace Minesweeper.Server.Logic
{
    public class SaveableGame : Game
    {
        private readonly DatabaseService _database;
        private readonly string _nickname;
        private readonly List<Gamemode> _officialGamemodes;

        public SaveableGame(List<Gamemode> officialGamemodes, Random random, Gamemode gamemode, DatabaseService database, string nickname) : base(random, gamemode)
        {
            _database = database;
            _nickname = nickname;
            _officialGamemodes = officialGamemodes;
        }

        public override void Play(int row, int column, FieldAction action)
        {
            var state = GameState;
            base.Play(row, column, action);

            if (state != GameState.Won && GameState == GameState.Won)
            {
                var official = _officialGamemodes.FirstOrDefault(g => g.Bombs == Gamemode.Bombs &&
                    g.Width == Gamemode.Width &&
                    g.Height == Gamemode.Height &&
                    g.Name == Gamemode.Name);

                if (official != null)
                {
                    _database.Add(Gamemode, _nickname, RoundTime, StopTime);
                }
            }
        }
    }
}
