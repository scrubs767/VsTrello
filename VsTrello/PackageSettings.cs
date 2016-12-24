using Scrubs.VisualStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Settings;
using Scrubs.MvvmWeak;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;

namespace VsTrello
{
    [Export(typeof(IPackageSettings))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class PackageSettings : NotifiableObject, IPackageSettings
    {
        Scrubs.VisualStudio.SettingsStore _settingsStore;

        [ImportingConstructor]
        public PackageSettings([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            var sm = new ShellSettingsManager(serviceProvider);
            _settingsStore = new Scrubs.VisualStudio.SettingsStore(sm.GetWritableSettingsStore(SettingsScope.UserSettings), VsTrelloPackage.AppName);
            LoadSettings();
        }

        public void Save()
        {
            SaveSettings();
        }

        string _applicationKey;
        public string ApplicationKey
        {
            get { return _applicationKey; }
            set { _applicationKey = value; RaisePropertyChanged(); }
        }

        string _token;
        public string Token
        {
            get { return _token; }
            set { _token = value; RaisePropertyChanged(); }
        }

        string _lastSearch;
        public string LastSearch
        {
            get { return _lastSearch; }
            set { _lastSearch = value; RaisePropertyChanged(); }
        }

        bool _showDetails;
        public bool ShowDetails
        {
            get { return _showDetails; }
            set { _showDetails = value; RaisePropertyChanged(); }
        }

        void LoadSettings()
        {
            ApplicationKey = "cee553af96c8146989fff7c325b8ef54";
            Token = (string)_settingsStore.Read("Token", null);
            LastSearch = (string)_settingsStore.Read("LastSearch", null);
            ShowDetails = (bool)_settingsStore.Read("ShowDetails", true);
        }

        void SaveSettings()
        {
           // _settingsStore.Write("ApplicationKey", ApplicationKey);
            _settingsStore.Write("Token", Token);
            _settingsStore.Write("LastSearch", LastSearch);
            _settingsStore.Write("ShowDetails", ShowDetails);
        }
    }
}
