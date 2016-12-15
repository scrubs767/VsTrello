using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrubs.MvvmWeak;
using System.ComponentModel.Composition;
using Manatee.Trello;
using System.Windows.Input;
using Scrubs.VisualStudio;
using System.Diagnostics;

namespace VsTrello
{
    [Export(typeof(ICardsListViewModel))]
    public class CardsListViewModel : ViewModelBase, ICardsListViewModel
    {
        IPackageSettings _settings;

        public CardsListViewModel()
        {
            SearchCommand = new RelayCommand(SearchExecute, CanSearchExecute);
            OpenCardCommand = new RelayCommand(OpenCardExecute, (_)=> { return true; });
            _settings = Services.DefaultExportProvider.GetExportedValue<IPackageSettings>();
            _searchString = _settings.LastSearch;
        }

        public Card SelectedCard { get; set; }
        private void OpenCardExecute(object obj)
        {
            if(SelectedCard != null)
            {
                Process.Start(SelectedCard.ShortUrl);
            }
        }

        public ICommand OpenCardCommand { get; set; }

        private bool CanSearchExecute(object arg)
        {
            if (_searchString != null && _searchString.Length > 0) return true;
            return false;
        }

        private async void SearchExecute(object obj)
        {
            _settings.LastSearch = _searchString;
            _settings.Save();
            Cards = await DoSearch(CardSearchString);
        }

        public ICommand SearchCommand { get; set; }

        private string _searchString = null;
        public string CardSearchString
        {
            get { return _searchString; }
            set { _searchString = value;}
        }

        IEnumerable<Card> _cards;
        public IEnumerable<Card> Cards
        {
            get
            {
                return _cards;
            }
            private set
            {
                _cards = value;
                RaisePropertyChanged("Cards");
            }
        }
        public static Task<IEnumerable<Card>> DoSearch(string terms)
        {
            return Task.Run(() =>
            {
                return new Search(terms, 50, SearchModelType.Cards).Cards;
            });
        }
    }
}
