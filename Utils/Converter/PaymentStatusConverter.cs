using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Constants;
using System.Globalization;

namespace OwlReadingRoom.Utils.Converter
{
    public class PaymentStatusConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is PaymentStatusEnum paymentStatus)
            {
                return paymentStatus switch
                {
                    PaymentStatusEnum.PAID => AppConstants.PaymentStatus.PAID,
                    PaymentStatusEnum.UNPAID => AppConstants.PaymentStatus.UNPAID,
                    PaymentStatusEnum.PARTIALLY_PAID => AppConstants.PaymentStatus.PARTIALLY_PAID,
                    PaymentStatusEnum.ADVANCED => AppConstants.PaymentStatus.ADVANCED,
                };
            }
            return "N/A";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string paymentStatusString)
            {
                return paymentStatusString switch
                {
                    AppConstants.PaymentStatus.PAID => PaymentStatusEnum.PAID,
                    AppConstants.PaymentStatus.UNPAID => PaymentStatusEnum.UNPAID,
                    AppConstants.PaymentStatus.PARTIALLY_PAID => PaymentStatusEnum.PARTIALLY_PAID,
                    AppConstants.PaymentStatus.ADVANCED => PaymentStatusEnum.ADVANCED
                };
            }
            return null;
        }
    }
}
