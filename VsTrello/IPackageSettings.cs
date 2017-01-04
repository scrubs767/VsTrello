using System;
using System.Collections.Generic;
using System.ComponentModel;
using VsTrello.UI;

namespace VsTrello
{
    public interface IPackageSettings
    {
        string ApplicationKey { get; }
        int MruLastSearchCount { get; set; }
        string Token { get; set; }
        IEnumerable<string> LastSearch { get; set; }
        void Save();
        bool ShowDetails { get; set; }
        IEnumerable<SearchListColumn>  SearchListColumns {get;set;}
        event PropertyChangedEventHandler PropertyChanged;

    }
}