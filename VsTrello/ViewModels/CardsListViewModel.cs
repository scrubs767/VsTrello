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
using VsTrello.Services;
using Microsoft.VisualStudio.Shell;

namespace VsTrello.ViewModels
{
    [Export(typeof(ICardsListViewModel))]
    public class CardsListViewModel : ViewModelBase, ICardsListViewModel
    {
        IPackageSettings _settings;
        IServiceProvider _serviceProvider;
        [ImportingConstructor]
        public CardsListViewModel([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            SearchCommand = new RelayCommand(SearchExecute, CanSearchExecute);
            OpenCardCommand = new RelayCommand(OpenCardExecute, (_)=> { return true; });
            LaunchBroswerCommand = new RelayCommand(OpenBrowser, (_) => { return true; });
            _settings = Scrubs.VisualStudio.Services.DefaultExportProvider.GetExportedValue<IPackageSettings>();
            _settings.PropertyChanged += _settings_PropertyChanged;
        }

        private void _settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Saved")
            {
                RaisePropertyChanged("Columns");
            }
            else if(e.PropertyName == "LastSearch")
            {
                RaisePropertyChanged("CardSearchStrings");
            }
        }

        public IEnumerable<VsTrello.UI.SearchListColumn> Columns { get { return _settings.SearchListColumns.Where(a=>a.IsChecked); } }
        private bool _IsProgressBarRunning = false;
        public bool IsProgressBarRunning { get { return _IsProgressBarRunning; } set { _IsProgressBarRunning = value; RaisePropertyChanged(); } }

        private void OpenBrowser(object obj)
        {
            Process.Start(obj.ToString());
        }

        public ICommand LaunchBroswerCommand { get; set; }
        public Card SelectedCard { get; set; }
        private void OpenCardExecute(object obj)
        {
            if(SelectedCard != null)
            {
                //Process.Start(SelectedCard.ShortUrl);
                var toolWindowManager = _serviceProvider.GetService(typeof(SToolWindowManager)) as IToolWindowManager;
                if (toolWindowManager != null)
                {
                    toolWindowManager.OpenCardEditorWindow(new CardViewModel(_serviceProvider, SelectedCard));
                }
            }
        }

        public ICommand OpenCardCommand { get; set; }

        private bool CanSearchExecute(object arg)
        {
            if (SelectedSearchString != null && SelectedSearchString.Count() > 0) return true;
            return false;
        }

        public string SelectedSearchString { get; set; }
        private async void SearchExecute(object obj)
        {
            IsProgressBarRunning = true;
            List<string> searches = new List<string>(_settings.LastSearch);
            if (!searches.Contains(SelectedSearchString))
            {
                searches.Add(SelectedSearchString);
                if (searches.Count() > 6) { searches = searches.AsEnumerable().Reverse().Take(6).Reverse().ToList(); }
                _settings.LastSearch = searches;
                _settings.Save();
            }
            
            Cards = await DoSearch(SelectedSearchString);
            IsProgressBarRunning = false;
        }

        public ICommand SearchCommand { get; set; }

        public IEnumerable<string> CardSearchStrings
        {
            get { return _settings.LastSearch; }
            set { _settings.LastSearch = value; }
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
            return System.Threading.Tasks.Task.Run(() =>
            {
                return new Search(terms, 50, SearchModelType.Cards).Cards;
            });
        }
    }
}
