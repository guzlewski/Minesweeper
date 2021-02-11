using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Minesweeper.Client.Logic;
using Minesweeper.Client.Logic.Interfaces;
using Minesweeper.Common.DTO;
using Minesweeper.Common.Enums;
using Minesweeper.Common.Requests;

namespace Minesweeper.Client.UI.Windows
{
    /// <summary>
    /// Interaction logic for GameScreen.xaml
    /// </summary>
    public partial class GameScreen : Window
    {
        private readonly ICommunicationHelper _communication;
        private readonly Timer _timer;
        private readonly IAssets _assets;

        public GameDto Game { get; private set; }
        public string GamemodeName { get; set; }
        public bool ConnectionLost { get; private set; }

        public GameScreen(Window window, ICommunicationHelper communication, string gamemodeName, GameDto game, IAssets assets)
        {
            Owner = window;
            InitializeComponent();
            _communication = communication;
            _timer = new Timer(TimeTextBlock);
            _assets = assets;
            Game = game;
            GamemodeName = gamemodeName;

            TimeTextBlock.FontFamily = _assets.Font;
            BombsTextBlock.FontFamily = _assets.Font;
            Initialize();
        }

        private void Initialize()
        {
            Grid.Rows = Game.Board.Height;
            Grid.Columns = Game.Board.Width;
            BombsTextBlock.Text = Game.Board.Bombs.ToString();
            TimeTextBlock.Text = 0.ToString();
            RestartButton.Source = _assets.GameStates[Game.GameState];

            AddFields();
            UpdateFields(Game.Board.Fields.ToList());
            UpdateWindow(Game);
        }

        private void AddFields()
        {
            var i = 0;

            foreach (var field in Game.Board.Fields)
            {
                var tag = new FieldTag
                {
                    Row = i / Game.Board.Width,
                    Column = i % Game.Board.Width,
                    Field = field
                };

                var item = new Image()
                {
                    Height = 25,
                    Width = 25,
                    Tag = tag
                };

                item.PreviewMouseLeftButtonUp += Field_Click;
                item.PreviewMouseRightButtonUp += Field_Click;
                item.PreviewMouseLeftButtonDown += Field_Enter;
                item.MouseEnter += Field_Enter;
                item.MouseLeave += Field_Leave;
                Grid.Children.Add(item);

                i++;
            }
        }

        private void UpdateFields(List<FieldDto> fields)
        {
            for (var i = 0; i < fields.Count; i++)
            {
                if (fields[i].State == FieldState.Open)
                {
                    ((Image)Grid.Children[i]).Source = _assets.FieldValues[fields[i].Value];
                }
                else
                {
                    ((Image)Grid.Children[i]).Source = _assets.FieldStates[fields[i].State];
                }

                Game.Board.Fields[i].State = fields[i].State;
                Game.Board.Fields[i].Value = fields[i].Value;
            }
        }

        private void UpdateWindow(GameDto game)
        {
            var bombs = game.Board.Bombs;
            var flags = game.Board.Fields.Where(x => x.State == FieldState.Flag).Count();

            BombsTextBlock.Text = (bombs - flags).ToString();
            RestartButton.Source = _assets.GameStates[game.GameState];

            if (Game.GameState == GameState.New && game.GameState == GameState.InProgress)
            {
                _timer.Start();
            }

            Game.GameState = game.GameState;

            if (Game.GameState == GameState.Won || Game.GameState == GameState.Lost)
            {
                _timer.Stop();
            }

            if (Game.GameState == GameState.Won)
            {
                MessageBox.Show($"Game won in {Math.Round(game.RoundTime.TotalSeconds, 3)} s.");
            }
        }

        private async void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            var request = new CreateGame
            {
                Gamemode = new GamemodeDto
                {
                    Width = Game.Board.Width,
                    Height = Game.Board.Height,
                    Bombs = Game.Board.Bombs,
                    Name = GamemodeName
                }
            };

            _timer.Stop();
            TimeTextBlock.Text = 0.ToString();

            var response = await _communication.SendAndRecieveAsync<GameDto>(request);

            if (response == null)
            {
                ConnectionLost = true;
                Close();
                return;
            }

            UpdateWindow(response);
            UpdateFields(response.Board.Fields);
        }

        private async void Field_Click(object sender, MouseButtonEventArgs e)
        {
            if (Game.GameState == GameState.Lost || Game.GameState == GameState.Won)
            {
                return;
            }

            var button = (FrameworkElement)sender;
            var tag = (FieldTag)button.Tag;

            PlayGame playgameRequest = new PlayGame
            {
                Row = tag.Row,
                Column = tag.Column
            };

            if (e.ChangedButton == MouseButton.Left && tag.Field.State == FieldState.Close)
            {
                playgameRequest.Action = FieldAction.Open;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                if (tag.Field.State == FieldState.Close)
                {
                    playgameRequest.Action = FieldAction.Flag;
                }
                else if (tag.Field.State == FieldState.Flag)
                {
                    playgameRequest.Action = FieldAction.Mark;
                }
                else if (tag.Field.State == FieldState.Mark)
                {
                    playgameRequest.Action = FieldAction.Reset;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }

            var response = await _communication.SendAndRecieveAsync<GameDto>(playgameRequest);

            if (response == null)
            {
                ConnectionLost = true;
                Close();
                return;
            }

            UpdateFields(response.Board.Fields);
            UpdateWindow(response);
        }

        private void Field_Enter(object sender, MouseEventArgs e)
        {
            if (Game.GameState == GameState.Lost || Game.GameState == GameState.Won)
            {
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed && ((FieldTag)((FrameworkElement)sender).Tag).Field.State == FieldState.Close)
            {
                ((Image)sender).Source = _assets.FieldStates[FieldState.Open];
            }
        }

        private void Field_Leave(object sender, MouseEventArgs e)
        {
            if (Game.GameState == GameState.Lost || Game.GameState == GameState.Won)
            {
                return;
            }

            var image = (Image)sender;
            var tag = (FieldTag)image.Tag;

            if (tag.Field.State == FieldState.Close && image.Source == _assets.FieldStates[FieldState.Open])
            {
                ((Image)sender).Source = _assets.FieldStates[FieldState.Close];
            }
        }
    }
}
