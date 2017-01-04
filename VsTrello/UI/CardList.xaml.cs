using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using VsTrello.ViewModels;

namespace VsTrello.UI
{
    [Export("CardsList", typeof(IPageComponent))]
    public partial class CardList : UserControl, IPageComponent
    {
        [ImportingConstructor]
        public CardList([Import(typeof(ICardsListViewModel))] ICardsListViewModel cardsListViewModel) 
        {
            InitializeComponent();
            DataContext = cardsListViewModel;
        }
    }
}
