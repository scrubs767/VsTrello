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
        public CardViewModel(Card card)
        {
            var actions = card.Actions.ToList();
            var types = actions.Select(c => c.Type);
            foreach(var action in actions)
            {
                var data = action.Data;
                var type = action.Type;
            }
            Card = card;
            Card.Updated += Card_Updated;
        }

        private void Card_Updated(Card arg1, IEnumerable<string> arg2)
        {
            RaisePropertyChanged("Card");
        }

        bool _showDetails = true;
        public bool ShowDetails { get { return _showDetails; } set { _showDetails = value; RaisePropertyChanged("Actions"); } }
        Card _card;
        public Card Card { get { return _card; } set { _card = value; RaisePropertyChanged(); } }
        public System.Collections.ObjectModel.ReadOnlyCollection<IAction> Actions
        {
            get
            {
                List<IAction> actions = new List<IAction>();
                var act = Card.Actions.Filter(ActionType.All).ToList();
                Card.Actions.Filter(ActionType.All).ToList().ForEach(a =>
                {
                    var action = getAction(a);
                    if(action != null)
                        actions.Add(action);
                });
                return new System.Collections.ObjectModel.ReadOnlyCollection<IAction>(actions.OrderByDescending(a=>a.Date).ToList());
            }
        }
        private IAction getAction(Manatee.Trello.Action action)
        {

            if (action.Type == ActionType.CommentCard)
            {
                return new Comment() { Text = action.Data.Text, Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl };
            }
            else if (action.Type == ActionType.UpdateCard)
            {
                if (!ShowDetails) return null;
                //return new Update(action.Data) { Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl };
                if (action.Data?.ListBefore != null)
                    return new SimpleAction() { Text = $"moved this card from {action.Data.ListBefore.Name} to {action.Data.ListAfter.Name}", Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl };
                return null;
            }
            else if (action.Type == ActionType.AddAttachmentToCard)
            {
                if (!ShowDetails) return null;
                return new SimpleAction() { Text = $"attached {action.Data.Attachment} to this card.", Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl };
            }

            else if (action.Type == ActionType.AddMemberToCard)
            {
                if (!ShowDetails) return null;
                return new SimpleAction() { Text = "was added to the card.", Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl };
            }
            else if (action.Type == ActionType.RemoveMemberFromCard)
            {
                if (!ShowDetails) return null;
                return new SimpleAction() { Text = "was removed from the card.", Creator = action.Creator.ToString(), Date = action.Date, MemberImage = action.Creator.AvatarUrl };
            }
            else
                return null;
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
    }

    public class Attachment : NotifiableObject, IAction
    {
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public string Creator { get; set; }
        public string MemberImage { get; set; }
    }

    public class SimpleAction : NotifiableObject, IAction
    {
        public string Text { get; set; }
        public DateTime? Date { get; set; }
        public string Creator { get; set; }
        public string MemberImage { get; set; }
    }
    
    public interface IAction
    {
        string Text { get; }
        DateTime? Date { get; }
        string Creator { get; }
        string MemberImage { get; set; }
    }
}
