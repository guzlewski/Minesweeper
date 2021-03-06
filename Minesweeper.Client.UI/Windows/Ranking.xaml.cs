﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Minesweeper.Common.DTO;

namespace Minesweeper.Client.UI.Windows
{
    /// <summary>
    /// Interaction logic for Ranking.xaml
    /// </summary>
    public partial class Ranking : Window
    {
        public Ranking(Window owner, List<RankingDto> rankings)
        {
            Owner = owner;
            InitializeComponent();

            if (rankings.Count() == 0)
            {
                var emptyPage = new RankingDto
                {
                    Gamemode = new GamemodeDto
                    {
                        Bombs = -1,
                        Height = -1,
                        Width = -1,
                        Name = "Empty"
                    },
                    Total = 0
                };

                rankings.Add(emptyPage);
            }

            Gamemodes.ItemsSource = rankings;

            var enumerator = Gamemodes.ItemsSource.GetEnumerator();
            enumerator.MoveNext();
            Gamemodes.SelectedItem = enumerator.Current;
        }

        private void Gamemodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var page = (RankingDto)e.AddedItems[0];

            RankingListView.ItemsSource = page.Achievements;
            Total.Content = page.Total;
        }
    }
}
