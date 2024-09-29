using System.Diagnostics;
using System.Windows.Input;
using PlayCenter.Models;

namespace PlayCenter.ViewModels
{
    public class GameDetailsViewModel : ViewModelBase
    {
        public Game SelectedGame { get; set; }

        public GameDetailsViewModel(Game game)
        {
            SelectedGame = game;
        }

        public ICommand LaunchGameCommand => new RelayCommand(LaunchGame);

        private void LaunchGame()
        {
            Process.Start(SelectedGame.ExecutablePath);
        }
    }
}
