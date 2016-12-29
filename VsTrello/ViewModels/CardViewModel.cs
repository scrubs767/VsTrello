using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manatee.Trello;
using System.Collections.ObjectModel;
using Scrubs.MvvmWeak;
using System.Diagnostics;

namespace VsTrello.ViewModels
{
    public class CardViewModel : ViewModelBase, ICardViewModel, IDisposable
    {
        IPackageSettings _settings;
        public CardViewModel(IServiceProvider serviceProvider, Card card)
        {
            _settings = Scrubs.VisualStudio.Services.DefaultExportProvider.GetExportedValue<IPackageSettings>();
            Card = card;
            Card.Updated += Card_Updated;
            Title = $"VsTrello - {Card.Name}";
            //ShowDetails = true;
        }

        private void Card_Updated(Card arg1, IEnumerable<string> arg2)
        {
            RaisePropertyChanged("Card");
            RaisePropertyChanged("Actions");
        }

        public bool ShowDetails //{ get; set; }
        {
            get
            {
                return _settings.ShowDetails;
            }
    set
            {
                _settings.ShowDetails = value; _settings.Save(); RaisePropertyChanged("Actions");
}
        }
        Card _card;
        public Card Card { get { return _card; } set { _card = value; RaisePropertyChanged(); } }
        public System.Collections.ObjectModel.ReadOnlyCollection<IAction> Actions
        {
            get
            {
                List<IAction> actions = new List<IAction>();

                Card.Actions.Filter( ShowDetails ? ActionType.All : ActionType.CommentCard ).ToList().ForEach(a =>
                {
                    var action = getAction(a);
                    if(action != null)
                        actions.Add(action);
                });
                return new System.Collections.ObjectModel.ReadOnlyCollection<IAction>(actions.OrderByDescending(a=>a.Date).ToList());
            }
        }

        public string Title { get; internal set; }

        private IAction getAction(Manatee.Trello.Action action)
        {

            if (action.Type == ActionType.CommentCard)
            {
                return new Comment() { Text = action.Data.Text, Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl, Initials = action.Creator.Initials };
            }
            else if (action.Type == ActionType.UpdateCard)
            {
                if (!ShowDetails) return null;
                //return new Update(action.Data) { Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl };
                if (action.Data?.ListBefore != null)
                    return new SimpleAction() { Text = $"moved this card from {action.Data.ListBefore.Name} to {action.Data.ListAfter.Name}", Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl, Initials = action.Creator.Initials };
                return null;
            }
            else if (action.Type == ActionType.AddAttachmentToCard)
            {
                if (!ShowDetails) return null;
                return new SimpleAction() { Text = $"attached {action.Data.Attachment} to this card.", Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl, Initials = action.Creator.Initials };
            }

            else if (action.Type == ActionType.AddMemberToCard)
            {
                if (!ShowDetails) return null;
                return new SimpleAction() { Text = "was added to the card.", Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl, Initials = action.Creator.Initials };
            }
            else if (action.Type == ActionType.RemoveMemberFromCard)
            {
                if (!ShowDetails) return null;
                return new SimpleAction() { Text = "was removed from the card.", Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl, Initials = action.Creator.Initials };
            }



            else if (action.Type == ActionType.CreateCard)
            {
                if (!ShowDetails) return null;
                return new SimpleAction() { Text = $"added this card to {action.Data.List}.", Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl, Initials = action.Creator.Initials };
            }

            else if (action.Type == ActionType.UpdateCheckItemStateOnCard)
            {
                if (!ShowDetails) return null;
                //var a = action.Data.Attachment;
                //var b = action.Data.Board;
                //var c = action.Data.BoardSource;
                //var d = action.Data.BoardTarget;
                //var e = action.Data.Card;
                //var f = action.Data.CardSource;
                //var g = action.Data.CheckItem;

                //var h = action.Data.CheckList;
                //var i = action.Data.LastEdited;
                //var j = action.Data.List;
                //var k = action.Data.ListAfter;
                //var l = action.Data.ListBefore;

                //var m = action.Data.Member;
                //var n = action.Data.OldDescription;
                //var o = action.Data.OldList;
                //var p = action.Data.OldPosition;
                //var q = action.Data.OldText;

                //var r = action.Data.Organization;
                //var s = action.Data.Text;
                //var t = action.Data.Value;
                //var u = action.Data.WasArchived;

                return new SimpleAction() { Text = $"{action.Data.CheckItem.State} {action.Data.CheckItem.Name}", Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl, Initials = action.Creator.Initials };
            }
            else if (action.Type == ActionType.AddChecklistToCard)
            {
                if (!ShowDetails) return null;
                return new SimpleAction() { Text = $"added {action.Data.CheckList} to this card.", Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl, Initials = action.Creator.Initials };
            }
            else if(action.Type == ActionType.DeleteAttachmentFromCard)
            {
                if (!ShowDetails) return null;
                return new SimpleAction() { Text = $"deleted {action.Data.Attachment} from this card.", Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl, Initials = action.Creator.Initials };
            }
            else
            {
                System.Diagnostics.Trace.WriteLine($"###### {action.Type.ToString()} Not found.");
                return null;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Card.Updated -= Card_Updated;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CardViewModel() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class Comment : NotifiableObject, IAction
    {
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public string Creator { get; set; }
        public string MemberImage { get; set; }
        public string Initials { get; set; }
    }

    public class Attachment : NotifiableObject, IAction
    {
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public string Creator { get; set; }
        public string MemberImage { get; set; }
        public string Initials { get; set; }
    }

    public class SimpleAction : NotifiableObject, IAction
    {
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public string Creator { get; set; }
        public string MemberImage { get; set; }
        public string Initials { get; set; }
    }
    
    public interface IAction
    {
        string Text { get; }
        DateTime? Date { get; }
        string Creator { get; }
        string MemberImage { get; set; }
        string Initials { get; set; }
    }
}
