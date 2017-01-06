using Manatee.Trello;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VsTrello.UI
{
    public class AttachmentToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Attachment)) return value;
            var attachment = (Attachment)value;

            // mimetype is not filled out on attachment, so i do this
            if (attachment.Url == null) return null;
            if (!_imageFormats.Contains(Path.GetExtension(attachment.Name).ToLower().Remove(0, 1))) return null;
            ImageSource img = new BitmapImage(new Uri(attachment.Url));
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        private static List<string> _imageFormats = new List<string>(){"png", "jpg", "jpeg", "bmp", "gif" };
    }

}
