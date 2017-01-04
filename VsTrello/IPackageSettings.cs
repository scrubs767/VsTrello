using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VsTrello
{
    public interface IPackageSettings
    {
        string ApplicationKey { get; }
        string Token { get; set; }
        IEnumerable<string> LastSearch { get; set; }
        void Save();
        bool ShowDetails { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}