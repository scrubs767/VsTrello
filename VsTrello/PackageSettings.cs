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
using RestSharp;

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

        IEnumerable<string> _lastSearch;
        public IEnumerable<string> LastSearch
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
            try
            {
                var s = (string)_settingsStore.Read("LastSearch", new List<string>());
                LastSearch = SimpleJson.DeserializeObject<IEnumerable<string>>(s);
            }
            catch
            {
                LastSearch = upgradeLastSearch();
            }
            ShowDetails = (bool)_settingsStore.Read("ShowDetails", true);
        }

        private IEnumerable<string> upgradeLastSearch()
        {
            var setting = (string)_settingsStore.Read("LastSearch", null);
            if (setting != null)
            {
                var ret = new List<string>();
                ret.Add(setting);
                _settingsStore.Write("LastSearch", ret);
                return ret;
            }
            return new List<string>();
        }

        void SaveSettings()
        {
           // _settingsStore.Write("ApplicationKey", ApplicationKey);
            _settingsStore.Write("Token", Token);
            _settingsStore.Write("LastSearch", SimpleJson.SerializeObject(LastSearch));
            _settingsStore.Write("ShowDetails", ShowDetails);
        }
    }
}
