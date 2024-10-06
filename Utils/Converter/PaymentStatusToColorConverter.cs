using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Constants;
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
            if (value is PaymentStatusEnum paymentStatus)
            {
                return paymentStatus switch
                {
                    PaymentStatusEnum.PAID => Color.FromArgb("#22C55E"),
                    PaymentStatusEnum.UNPAID => Color.FromArgb("#EF4444"),
                    PaymentStatusEnum.ADVANCED => Color.FromArgb("#FACC15"),
                    PaymentStatusEnum.PARTIALLY_PAID => Color.FromArgb("#EF6F44")
                };
            }
            return Color.FromArgb("#FFFFFF"); // Default to white if status is unknown
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
