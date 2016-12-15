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
    [Export("CardsList", typeof(IPageComponent))]
    public partial class CardList : UserControl, IPageComponent
    {
        //private CardsListViewModel cardsListViewModel;

        //private CardList()
        //{
            
        //}

        [ImportingConstructor]
        public CardList(ICardsListViewModel cardsListViewModel) 
        {
            InitializeComponent();
            DataContext = cardsListViewModel;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ((CardsListViewModel)DataContext).OpenCardCommand.Execute(null);
        }
    }
}
