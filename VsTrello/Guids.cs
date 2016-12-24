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
        public const string InitializationNeededPage = "AF3E0682-29F1-45F0-A623-E62EAC44B58F";
        public const string OptionsPage = "C93327D5-7066-40ED-A89F-795BDCAFE2DC";
        public const string ToolWindowManagerInterface = "79CC1F6C-3F4A-4799-B2D8-80410C84039A";
        public const string ToolWindowManagerService = "EE86A4F0-767B-4968-8D21-0B02F73E012D";
        public static Guid ToGuid(this string g)
        {
            return Guid.Parse(g);
        }
    }
}
