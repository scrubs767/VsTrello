using Manatee.Trello;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VsTrello.UI
{
    public class CheckListCompletionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is CheckItemCollection)
            {
                CheckItemCollection items = value as CheckItemCollection;
                double count = items.Count();
                double completed = 0;
                foreach(CheckItem item in items)
                {
                    if (item.State == CheckItemState.Complete) completed++;
                }
                if (completed == 0) return 0;
                return (completed / count) * 100;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
