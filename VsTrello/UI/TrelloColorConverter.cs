using Manatee.Trello;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace VsTrello.UI
{
    public class TrelloColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LabelColor)
            {
                switch (((LabelColor)value))
                {
                    case LabelColor.Green:
                        return Brushes.Green;
                    case LabelColor.Yellow:
                        return Brushes.Yellow;
                    case LabelColor.Orange:
                        return Brushes.Orange;
                    case LabelColor.Red:
                        return Brushes.Red;
                    case LabelColor.Purple:
                        return Brushes.Purple;
                    case LabelColor.Blue:
                        return Brushes.Blue;
                    case LabelColor.Pink:
                        return Brushes.Pink;
                    case LabelColor.Sky:
                        return Brushes.SkyBlue;
                    case LabelColor.Lime:
                        return Brushes.Lime;
                    case LabelColor.Black:
                        return Brushes.Black;
                }                
            }
            return Colors.Wheat;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
