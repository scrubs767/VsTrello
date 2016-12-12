using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsTrello
{
    public static class Guids
    {
        public const string Package = "62692b59-bbd0-453f-be0a-2048db42dfde";
        public const string TrelloPage = "B0F37796-033C-49EC-B126-4190C4935C73";
        public const string TrelloNavigationItem = "8698771F-9B65-4324-8176-FCC394BF4A0A";

        public static Guid ToGuid(this string g)
        {
            return Guid.Parse(g);
        }
    }
}
