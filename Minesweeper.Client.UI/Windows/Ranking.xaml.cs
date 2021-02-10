using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            Gamemodes.SelectedItem = Gamemodes.Items[0];
        }

        private void Gamemodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var page = (RankingDto)e.AddedItems[0];

            RankingListView.ItemsSource = page.Achievements;
            Total.Content = page.Total;
        }
    }
}
