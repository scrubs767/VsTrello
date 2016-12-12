using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrubs.MvvmWeak;
namespace VsTrello
{
    public class CardsListViewModel : ViewModelBase
    {
        private string _searchString = null;
        public string CardSearchString
        {
            get { return _searchString; }
            set { _searchString = value; }
        }
    }
}
