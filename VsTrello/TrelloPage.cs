using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Controls;
using Scrubs.TeamExplorer;

namespace VsTrello
{
    [TeamExplorerPage(Guids.TrelloPage, Undockable = true)]
    public class TrelloPage : TeamExplorerPageBase
    {
        public override void Initialize(object sender, PageInitializeEventArgs e)
        {
            base.Initialize(sender, e);
            Title = "VsTrello";
            PageContent = new CardList(new CardsListViewModel());
        }

    }
}
