using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Minesweeper.Client.Logic;
using Minesweeper.Client.Logic.Interfaces;
using Minesweeper.Common.DTO;
using Minesweeper.Common.Requests;

namespace Minesweeper.Client.UI.Windows
{
    /// <summary>
    /// Interaction logic for GameSettings.xaml
    /// </summary>
    public partial class GameSettings : Window
    {
        private readonly ICommunicationHelper _communication;
        private readonly IServiceProvider _provider;

        public GameSettings(ICommunicationHelper communication, Gamemodes gamemodes, IServiceProvider provider)
        {
            InitializeComponent();
            _communication = communication;
            _provider = provider;

            LockSliders();
            LevelsList.ItemsSource = gamemodes.List;
            LevelsList.SelectedItem = gamemodes.List.FirstOrDefault();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (WidthSlider == null || HeightSlider == null || BombsSlider == null)
            {
                return;
            }

            var max = (WidthSlider.Value - 1) * (HeightSlider.Value - 1);

            if (BombsSlider.Value > max)
            {
                BombsSlider.Value = max;
            }

            BombsSlider.Maximum = max;
        }

        private void LevelsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lvl = (GamemodeDto)e.AddedItems[0];

            if (lvl.Width == 0 || lvl.Height == 0 || lvl.Bombs == 0)
            {
                UnlockSliders();
            }
            else
            {
                WidthSlider.Value = lvl.Width;
                HeightSlider.Value = lvl.Height;
                BombsSlider.Value = lvl.Bombs;

                LockSliders();
            }
        }

        private void LockSliders()
        {
            WidthSlider.IsEnabled = false;
            HeightSlider.IsEnabled = false;
            BombsSlider.IsEnabled = false;
        }

        private void UnlockSliders()
        {
            WidthSlider.IsEnabled = true;
            HeightSlider.IsEnabled = true;
            BombsSlider.IsEnabled = true;
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var request = new CreateGame
            {
                Gamemode = new GamemodeDto
                {
                    Width = (int)WidthSlider.Value,
                    Height = (int)HeightSlider.Value,
                    Bombs = (int)BombsSlider.Value,
                    Name = ((GamemodeDto)LevelsList.SelectedItem).Name
                }
            };

            var response = await _communication.SendAndRecieveAsync<GameDto>(request);

            if (response == null)
            {
                _provider.GetRequiredService<ServerSettings>().Show();
                Close();
                return;
            }

            using (var scope = _provider.CreateScope())
            {
                var gameScreen = new GameScreen(this, _communication, response, _provider.GetRequiredService<IAssets>());
                gameScreen.ShowDialog();

                if (gameScreen.ConnectionLost)
                {
                    _provider.GetRequiredService<ServerSettings>().Show();
                    Close();
                }
            }
        }

        private async void RankingButton_Click(object sender, RoutedEventArgs e)
        {
            var response = await _communication.SendAndRecieveAsync<List<RankingDto>>(new GetRanking());

            if (response == null)
            {
                _provider.GetRequiredService<ServerSettings>().Show();
                Close();
                return;
            }

            new Ranking(this, response).ShowDialog();
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            _provider.GetRequiredService<ServerSettings>().Show();
            Close();
        }
    }
}
