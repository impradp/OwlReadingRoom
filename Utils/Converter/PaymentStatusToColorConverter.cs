using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Utils.Converter
{
    public class PaymentStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string paymentStatus)
            {
                return paymentStatus.ToLower() == "paid" ? Color.FromArgb("#22C55E") : Color.FromArgb("#EF4444");
            }
            return Color.FromArgb("#EF4444"); // Default to red if status is unknown
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
