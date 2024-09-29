using System.Collections.ObjectModel;
using System.Windows.Input;
using PlayCenter.Models;

namespace PlayCenter.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Game> Games { get; set; }

        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        private bool _isDetailVisible;
        public bool IsDetailVisible
        {
            get => _isDetailVisible;
            set { _isDetailVisible = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            Games = new ObservableCollection<Game>
            {
                new Game { Name = "Tetris", ImagePath = "Images/tetris.png", Description = "A classic tile-matching puzzle game.", ExecutablePath = "Tetris.exe" },
                new Game { Name = "Calendar", ImagePath = "Images/calendar.png", Description = "A simple calendar application.", ExecutablePath = "Calender.exe" },
                new Game { Name = "Calendar", ImagePath = "Images/calendar.png", Description = "A simple calendar application.", ExecutablePath = "Calender.exe" },
                new Game { Name = "Calendar", ImagePath = "Images/calendar.png", Description = "A simple calendar application.", ExecutablePath = "Calender.exe" },
                new Game { Name = "Calendar", ImagePath = "Images/calendar.png", Description = "A simple calendar application.", ExecutablePath = "Calender.exe" },
                // Add all other games here...
            };

            CurrentView = new GameListViewModel(Games);
        }

        public ICommand GameSelectedCommand => new RelayCommand<Game>(ShowGameDetails);

        private void ShowGameDetails(Game selectedGame)
        {
            CurrentView = new GameDetailsViewModel(selectedGame);
            IsDetailVisible = true;
        }

        public ICommand BackCommand => new RelayCommand(BackToGameList);

        private void BackToGameList()
        {
            CurrentView = new GameListViewModel(Games);
            IsDetailVisible = false;
        }
    }
}
