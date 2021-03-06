﻿using Scrubs.VisualStudio;
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
using VsTrello.UI;
using RestSharp;
using System.Windows.Input;

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
            this.RaisePropertyChanged("Saved");
        }

        IEnumerable<SearchListColumn> _searchListColumns;
        public IEnumerable<SearchListColumn> SearchListColumns
        {
            get { return _searchListColumns; }
            set { _searchListColumns = value; this.RaisePropertyChanged(); }
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

        int _mruLastSearchCount;
        public int MruLastSearchCount
        {
            get { return _mruLastSearchCount; }
            set { _mruLastSearchCount = value; RaisePropertyChanged(); }
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
            MruLastSearchCount = (int)_settingsStore.Read("MruLastSearchCount", 6);
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
            var temp = (string)_settingsStore.Read("SearchListColumns", null);
            if (temp != null && temp.Length != 0)
            {
                SearchListColumns = SimpleJson.DeserializeObject<IEnumerable<SearchListColumn>>(temp);
            }
            else
            {
                SearchListColumns = new List<SearchListColumn>() {
                { new SearchListColumn() { DisplayMember="Board", HeaderText = "Board", IsChecked = false} },
                { new SearchListColumn() { DisplayMember="CreationDate", HeaderText = "Created", IsChecked = false} },
                { new SearchListColumn() { DisplayMember="Description", HeaderText = "Description", IsChecked = false} },
                { new SearchListColumn() { DisplayMember="DueDate", HeaderText = "Due", IsChecked = false} },
                { new SearchListColumn() { DisplayMember="Id", HeaderText = "Id", IsChecked = true} },
                { new SearchListColumn() { DisplayMember="IsArchived", HeaderText = "Archived", IsChecked = false} },
                { new SearchListColumn() { DisplayMember="IsSubscribed", HeaderText = "Subscribed", IsChecked = false} },
                { new SearchListColumn() { DisplayMember="LastActivity", HeaderText = "Last Activity", IsChecked = false} },
                { new SearchListColumn() { DisplayMember="List", HeaderText = "List", IsChecked = true} },
                { new SearchListColumn() { DisplayMember="Name", HeaderText = "Name", IsChecked = true} },
                { new SearchListColumn() { DisplayMember="Position", HeaderText = "Position", IsChecked = false} },
                { new SearchListColumn() { DisplayMember="ShortId", HeaderText = "Short Id", IsChecked = false} },
            };
            }
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
            _settingsStore.Write("MruLastSearchCount", MruLastSearchCount);
            _settingsStore.Write("Token", Token);
            _settingsStore.Write("LastSearch", SimpleJson.SerializeObject(LastSearch));
            _settingsStore.Write("ShowDetails", ShowDetails);
            _settingsStore.Write("SearchListColumns", SimpleJson.SerializeObject(SearchListColumns));
        }
    }
}
