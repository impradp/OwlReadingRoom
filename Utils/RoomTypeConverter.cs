using OwlReadingRoom.Models;
using System.Globalization;

namespace OwlReadingRoom.Utils;

public class RoomTypeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        RoomType roomType = (RoomType)value;
        return roomType switch
        {
            RoomType.AC => "AC Room",
            RoomType.NON_AC => "Non-AC Room",
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string roomTypeString = value as string;
        return roomTypeString switch
        {
            "AC Room" => RoomType.AC,
            "Non-AC Room" => RoomType.NON_AC
        };

    }
}
