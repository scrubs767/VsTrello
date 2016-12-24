using System.ComponentModel.Composition;
using System.Windows.Controls;
using VsTrello.ViewModels;

namespace VsTrello.UI
{
    [Export("CardsList", typeof(IPageComponent))]
    public partial class CardList : UserControl, IPageComponent
    {
        //private CardsListViewModel cardsListViewModel;

        //private CardList()
        //{
            
        //}

        [ImportingConstructor]
        public CardList([Import(typeof(ICardsListViewModel))] ICardsListViewModel cardsListViewModel) 
        {
            InitializeComponent();
            DataContext = cardsListViewModel;
        }

        //private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    ((CardsListViewModel)DataContext).OpenCardCommand.Execute(null);
        //}
    }
}
