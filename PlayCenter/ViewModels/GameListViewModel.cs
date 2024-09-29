using System.Collections.ObjectModel;
using PlayCenter.Models;

namespace PlayCenter.ViewModels
{
    public class GameListViewModel : ViewModelBase
    {
        public ObservableCollection<Game> Games { get; }

        public GameListViewModel(ObservableCollection<Game> games)
        {
            Games = games;
        }
    }
}
