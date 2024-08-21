using System.Globalization;

namespace OwlReadingRoom.Utils
{
    public class MenuColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string selectedMenu = value as string;
            string menuItem = parameter as string;
            return selectedMenu == menuItem ? Color.FromHex("#2374E1") : Color.FromHex("#07003B");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
