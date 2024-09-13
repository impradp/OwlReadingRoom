using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Constants;
using System.Globalization;

namespace OwlReadingRoom.Utils;

public class RoomTypeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        RoomType roomType = (RoomType)value;
        return roomType switch
        {
            RoomType.AC => ResourceConstants.RoomConstants.AcRoom,
            RoomType.NON_AC => ResourceConstants.RoomConstants.NonAcRoom,
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string roomTypeString = value as string;
        return roomTypeString switch
        {
            ResourceConstants.RoomConstants.AcRoom => RoomType.AC,
            ResourceConstants.RoomConstants.NonAcRoom => RoomType.NON_AC
        };

    }
}
