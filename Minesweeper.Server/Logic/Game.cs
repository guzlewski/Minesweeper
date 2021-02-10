using System;
using System.Collections.Generic;
using System.Linq;
using Minesweeper.Common.Enums;

namespace Minesweeper.Server.Logic
{
    public class Game
    {
        private readonly Random _random;

        public Board Board { get; set; }
        public GameState GameState { get; set; }
        public Gamemode Gamemode { get; set; }
        public TimeSpan RoundTime { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset StopTime { get; set; }
        public int OpenFields { get; set; }

        public Game(Random random, Gamemode gamemode)
        {
            _random = random;

            Board = new Board
            {
                Width = gamemode.Width,
                Height = gamemode.Height,
                Bombs = gamemode.Bombs,
                Fields = new List<Field>()
            };
            GameState = GameState.New;
            Gamemode = gamemode;

            for (int i = 0; i < Gamemode.Width * Gamemode.Height; i++)
            {
                var field = new Field
                {
                    State = FieldState.Close,
                    Value = 0,
                    IsBomb = false
                };

                Board.Fields.Add(field);
            }
        }

        public void Play(int row, int column, FieldAction action)
        {
            if (GameState == GameState.Lost || GameState == GameState.Won ||
                row < 0 || row >= Gamemode.Height ||
                column < 0 || column >= Gamemode.Width)
            {
                return;
            }

            if (GameState == GameState.New && action == FieldAction.Open)
            {
                Setup(row, column);
            }

            switch (action)
            {
                case FieldAction.Open:
                    Open(row, column);
                    CheckGamestate(row, column);
                    break;
                case FieldAction.Flag:
                    Flag(row, column);
                    break;
                case FieldAction.Mark:
                    Mark(row, column);
                    break;
                case FieldAction.Reset:
                    Reset(row, column);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void Setup(int row, int column)
        {
            Board.Fields[row * Board.Width + column].State = FieldState.Open;

            DrawBombs();
            CalculateFieldsValue();

            Board.Fields[row * Board.Width + column].State = FieldState.Close;

            GameState = GameState.InProgress;
            StartTime = DateTimeOffset.Now;
        }

        private void DrawBombs()
        {
            var bombFields = Board.Fields
                .Where(field => field.State == FieldState.Close)
                .OrderBy(x => _random.Next())
                .Take(Board.Bombs);

            foreach (var field in bombFields)
            {
                field.IsBomb = true;
            }
        }

        private void CalculateFieldsValue()
        {
            for (int row = 0; row < Board.Height; row++)
            {
                for (int column = 0; column < Board.Width; column++)
                {
                    if (Board.Fields[row * Board.Width + column].IsBomb)
                    {
                        IncrementFieldValue(row - 1, column - 1);
                        IncrementFieldValue(row - 1, column);
                        IncrementFieldValue(row - 1, column + 1);
                        IncrementFieldValue(row, column - 1);
                        IncrementFieldValue(row, column + 1);
                        IncrementFieldValue(row + 1, column - 1);
                        IncrementFieldValue(row + 1, column);
                        IncrementFieldValue(row + 1, column + 1);
                    }
                }
            }
        }

        private void IncrementFieldValue(int row, int column)
        {
            if (row < 0 || row >= Board.Height || column < 0 || column >= Board.Width)
            {
                return;
            }

            Board.Fields[row * Board.Width + column].Value++;
        }

        private void Open(int row, int column)
        {
            var field = Board.Fields[row * Board.Width + column];

            if (field.State == FieldState.Close)
            {
                if (field.IsBomb)
                {
                    field.State = FieldState.Explode;
                }
                else
                {
                    OpenRecursively(row, column);
                }
            }
        }

        private void OpenRecursively(int row, int column)
        {
            if (row < 0 || row >= Board.Height || column < 0 || column >= Board.Width)
            {
                return;
            }

            var field = Board.Fields[row * Board.Width + column];

            if (field.State != FieldState.Close && field.State != FieldState.Mark)
            {
                return;
            }

            field.State = FieldState.Open;
            OpenFields++;

            if (field.Value == 0)
            {
                OpenRecursively(row - 1, column - 1);
                OpenRecursively(row - 1, column);
                OpenRecursively(row - 1, column + 1);
                OpenRecursively(row, column - 1);
                OpenRecursively(row, column + 1);
                OpenRecursively(row + 1, column + 1);
                OpenRecursively(row + 1, column);
                OpenRecursively(row + 1, column + 1);
            }
        }

        private void CheckGamestate(int row, int column)
        {
            var field = Board.Fields[row * Board.Width + column];

            if (field.State == FieldState.Explode)
            {
                GameState = GameState.Lost;
                StopTime = DateTimeOffset.Now;
                RoundTime = StopTime - StartTime;
                ShowBobmb();
            }
            else if (OpenFields + Board.Bombs == Board.Fields.Count)
            {
                GameState = GameState.Won;
                StopTime = DateTimeOffset.Now;
                RoundTime = StopTime - StartTime;
            }
        }

        private void ShowBobmb()
        {
            foreach (var field in Board.Fields)
            {
                if (field.IsBomb && (field.State == FieldState.Close || field.State == FieldState.Mark))
                {
                    field.State = FieldState.Bomb;
                }
                else if (field.State == FieldState.Flag && !field.IsBomb)
                {
                    field.State = FieldState.Miss;
                }
            }
        }

        private void Flag(int row, int column)
        {
            var field = Board.Fields[row * Board.Width + column];

            if (field.State == FieldState.Close || field.State == FieldState.Mark)
            {
                field.State = FieldState.Flag;
            }
        }

        private void Mark(int row, int column)
        {
            var field = Board.Fields[row * Board.Width + column];

            if (field.State == FieldState.Close || field.State == FieldState.Flag)
            {
                field.State = FieldState.Mark;
            }
        }

        private void Reset(int row, int column)
        {
            var field = Board.Fields[row * Board.Width + column];

            if (field.State == FieldState.Flag || field.State == FieldState.Mark)
            {
                field.State = FieldState.Close;
            }
        }
    }
}
