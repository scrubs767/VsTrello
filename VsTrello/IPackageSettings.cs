using System;
using System.ComponentModel;

namespace VsTrello
{
    public interface IPackageSettings
    {
        string ApplicationKey { get; }
        string Token { get; set; }
        string LastSearch { get; set; }
        void Save();
        event PropertyChangedEventHandler PropertyChanged;
    }
}