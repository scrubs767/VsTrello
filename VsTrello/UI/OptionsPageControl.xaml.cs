using Scrubs.VisualStudio;
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

        public string TokenRequestUrl
        {
            get
            {
                return $"https://trello.com/1/authorize?key={_settings.ApplicationKey }&scope=read%2Cwrite&name=VsTrello&expiration=never&response_type=token";
                //return "https://trello.com/1/authorize?expiration=never&name=VsTrello&key=" + _settings.ApplicationKey;
            }
        }

        public string Token { get { return _settings.Token; } set { _settings.Token = value; _settings.Save(); } }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (sender.GetType() != typeof(Hyperlink))
                return;
            string link = ((Hyperlink)sender).NavigateUri.ToString();
            Process.Start(link);
        }
    }
}
