using System.Collections.Generic;
using Manatee.Trello;

namespace VsTrello
{
    public interface ITrelloWrapper
    {
        bool IsAuthenticated { get; }
        IEnumerable<Card> Search(string query);
    }
}