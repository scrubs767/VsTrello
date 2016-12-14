using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using Scrubs.VisualStudio;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsTrello
{
    [Export(typeof(ITrelloWrapper))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class TrelloWrapper : ITrelloWrapper
    {
        IPackageSettings _settings;
        ManateeSerializer _serializer;
        public TrelloWrapper()
        {
            _serializer = new ManateeSerializer();
            TrelloConfiguration.Serializer = _serializer;
            TrelloConfiguration.Deserializer = _serializer;
            TrelloConfiguration.JsonFactory = new ManateeFactory();
            TrelloConfiguration.RestClientProvider = new WebApiClientProvider();

            _settings = Services.DefaultExportProvider.GetExportedValue<IPackageSettings>();
            _settings.PropertyChanged += _settings_PropertyChanged;
            TrelloAuthorization.Default.AppKey = _settings.ApplicationKey;
            TrelloAuthorization.Default.UserToken = _settings.Token;

        }

        private void _settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            TrelloAuthorization.Default.AppKey = _settings.ApplicationKey;
            TrelloAuthorization.Default.UserToken = _settings.Token;
        }

        public bool IsAuthenticated
        {
            get
            {
                try
                {
                    var foo = Member.Me;

                    if (Member.Me.IsConfirmed == true)
                        return true;
                    else
                        return false;
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
        }

        public IEnumerable<Card> Search(string query)
        {
            return null;
        }
    }
}
