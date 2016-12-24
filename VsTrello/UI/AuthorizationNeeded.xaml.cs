using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace VsTrello.UI
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
