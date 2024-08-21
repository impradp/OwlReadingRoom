using System.Globalization;
using static OwlReadingRoom.MainView;

namespace OwlReadingRoom.Utils
{
    public class SubmenuTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentSubmenu = (SelectedSubMenu)value;
            var submenuName = parameter as string;

            if (currentSubmenu.ToString() == submenuName)
            {
                return Color.FromHex("#3B82F6");  // Selected text color
            }
            return Color.FromHex("#1E293B");  // Default text color
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
