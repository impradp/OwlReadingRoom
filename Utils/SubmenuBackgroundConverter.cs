using System.Globalization;
using static OwlReadingRoom.MainView;

namespace OwlReadingRoom.Utils
{
    public class SubmenuBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentSubmenu = (SelectedSubMenu)value;
            var submenuName = parameter as string;

            if (currentSubmenu.ToString() == submenuName)
            {
                return Color.FromHex("#EFF6FF");  // Selected color
            }
            return Colors.White;  // Default color
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
