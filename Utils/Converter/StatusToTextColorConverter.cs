using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Utils.Converter
{
    public class StatusToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status.ToLower() == "active" ? Color.FromArgb("#22C55E") : Color.FromArgb("#3B82F6");
            }
            return Color.FromArgb("#000000");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
