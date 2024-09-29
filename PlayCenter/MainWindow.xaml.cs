using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PlayCenter.Models;
using PlayCenter.Views;
using PlayCenter.ViewModels;

namespace PlayCenter
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Game> Games { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Games = new ObservableCollection<Game>
            {
                new Game { Name = "Tetris", ImagePath = "Images/tetrist.png", Description = "A classic tile-matching puzzle game.", ExecutablePath = "Tetris.exe" },
                new Game { Name = "Calendar", ImagePath = "Images/calendar.png", Description = "A simple calendar application.", ExecutablePath = "Calender.exe" },
                new Game { Name = "TicTac", ImagePath = "Images/ticTac.png", Description = "A simple calendar application.", ExecutablePath = "Tic-Tac.exe" },
                new Game { Name = "Snake", ImagePath = "Images/snake.png", Description = "A simple calendar application.", ExecutablePath = "Snake.exe" },
                new Game { Name = "Pacman", ImagePath = "Images/pacman.png", Description = "A simple calendar application.", ExecutablePath = "Pacman.exe" },
                new Game { Name = "Weather", ImagePath = "Images/rain_cloud.png", Description = "A simple calendar application.", ExecutablePath = "Weather.exe" },
            };

            DataContext = this;
        }

        public ICommand GameSelectedCommand => new RelayCommand<Game>(GameSelected);

        private void GameSelected(Game selectedGame)
        {
            var gameDetailsWindow = new GameDetails
            {
                DataContext = new GameDetailsViewModel(selectedGame)
            };
            gameDetailsWindow.ShowDialog();
        }
    }
}
