using System.Globalization;

namespace OwlReadingRoom.Utils.Converter
{
    public class StatusToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status.ToLower() == "active" ? Color.FromHex("#F6FDF9") : Color.FromHex("#F5F9FF");
            }
            return Color.FromHex("#FFFFFF");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
