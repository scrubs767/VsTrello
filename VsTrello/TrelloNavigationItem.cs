﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scrubs.TeamExplorer;
using Microsoft.TeamFoundation.Controls;
using System.ComponentModel.Composition;
using VsTrello.Properties;
using Microsoft.VisualStudio.Shell;
using Scrubs.VisualStudio;
using VsTrello.Utility;
namespace VsTrello
{
    [TeamExplorerNavigationItem(Guids.TrelloNavigationItem, 1500, TargetPageId = Guids.TrelloPage)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TrelloNavigationItem : TeamExplorerNavigationItemBase
    {
        [ImportingConstructor]
        public TrelloNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Image = Resources.trello_mark_blue;
            ArgbColor = VsTrello.Utility.Colors.TrelloBlue.ToInt32();
            IsVisible = true;
            IsEnabled = true;          
            Text = "VsTrello";
        }

        public override void Execute()
        {
            var service = this.GetService<ITeamExplorer>();
            if (service == null)
            {
                return;
            }
            service.NavigateToPage(Guids.TrelloPage.ToGuid(), null);
        }
    }
}
