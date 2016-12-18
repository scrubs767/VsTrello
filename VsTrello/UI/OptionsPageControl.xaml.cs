using Scrubs.VisualStudio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VsTrello
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
            _settings = Services.DefaultExportProvider.GetExportedValue<IPackageSettings>();
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
