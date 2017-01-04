using Scrubs.MvvmWeak;
using Scrubs.VisualStudio;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace VsTrello.UI
{
    /// <summary>
    /// Interaction logic for OptionsPageControl.xaml
    /// </summary>
    public partial class OptionsPageControl : UserControl
    {
        IPackageSettings _settings;
        public OptionsPageControl()
        {
            InitializeComponent();
            _settings = Scrubs.VisualStudio.Services.DefaultExportProvider.GetExportedValue<IPackageSettings>();
            DataContext = this;
        }

        public IEnumerable<SearchListColumn> SearchListColumns { get { return _settings.SearchListColumns; } set { _settings.SearchListColumns = value; _settings.Save(); } }

        public string TokenRequestUrl
        {
            get
            {
                return $"https://trello.com/1/authorize?key={_settings.ApplicationKey }&scope=read%2Cwrite&name=VsTrello&expiration=never&response_type=token";
                //return "https://trello.com/1/authorize?expiration=never&name=VsTrello&key=" + _settings.ApplicationKey;
            }
        }

        public int MruLastSearchCount { get { return _settings.MruLastSearchCount; } set { _settings.MruLastSearchCount = value; _settings.Save(); } }

        public string Token { get { return _settings.Token; } set { _settings.Token = value; _settings.Save(); } }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (sender.GetType() != typeof(Hyperlink))
                return;
            string link = ((Hyperlink)sender).NavigateUri.ToString();
            Process.Start(link);
        }
    }
    public class SearchListColumn : NotifiableObject
    {
        bool _isChecked;
        public bool IsChecked { get { return _isChecked; } set { _isChecked = value; RaisePropertyChanged(); } }
        public string HeaderText { get; set; }
        public string DisplayMember { get; set; }
    }
}
