using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Controls;
using Scrubs.TeamExplorer;
using Scrubs.VisualStudio;
using System.ComponentModel;

namespace VsTrello
{
    [TeamExplorerPage(Guids.TrelloPage, Undockable = true)]
    public class TrelloPage : TeamExplorerPageBase
    {
        IPackageSettings _settings;

        public override void Initialize(object sender, PageInitializeEventArgs e)
        {
            base.Initialize(sender, e);
            Title = "VsTrello";
            _settings = Scrubs.VisualStudio.Services.DefaultExportProvider.GetExportedValue<IPackageSettings>();
            _settings.PropertyChanged += _settings_PropertyChanged;
            Refresh();
        }

        private void _settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Token")
                Refresh();
        }
        public override void Refresh()
        {
            base.Refresh();
            ITrelloWrapper wrapper = Scrubs.VisualStudio.Services.DefaultExportProvider.GetExportedValue<ITrelloWrapper>();
            if (wrapper.IsAuthenticated)
                PageContent = Scrubs.VisualStudio.Services.DefaultExportProvider.GetExportedValue<IPageComponent>("CardsList");
            else
                PageContent = Scrubs.VisualStudio.Services.DefaultExportProvider.GetExportedValue<IPageComponent>("AuthNeededComponent");
        }
    }
}
