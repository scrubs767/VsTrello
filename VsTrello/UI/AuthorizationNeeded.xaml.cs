using Microsoft.VisualStudio.Shell;
using Scrubs.VisualStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
    [Export("AuthNeededComponent", typeof(IPageComponent))]
    public partial class AuthorizationNeeded : UserControl, IPageComponent
    {
        public AuthorizationNeeded()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            VsTrelloPackage.Package.ShowOptionPage(typeof(OptionPageCustom));
        }
    }
}
