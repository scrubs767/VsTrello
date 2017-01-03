using System;
using System.Collections.Generic;
using System.ComponentModel;
using VsTrello.UI;

namespace VsTrello
{
    public interface IPackageSettings
    {
        string ApplicationKey { get; }
        string Token { get; set; }
        string LastSearch { get; set; }
        void Save();
        bool ShowDetails { get; set; }
        IEnumerable<SearchListColumn>  SearchListColumns {get;set;}
        event PropertyChangedEventHandler PropertyChanged;
    }
}