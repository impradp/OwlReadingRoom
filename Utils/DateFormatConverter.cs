using System.Globalization;

namespace OwlReadingRoom.Utils;

public class DateFormatConverter : IValueConverter
{
    /// <summary>
    /// Converts a DateTime value to its string representation in "MM/dd/yyyy" format.
    /// </summary>
    /// <param name="value">The DateTime value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>The string representation of the date in "MM/dd/yyyy" format, or null if conversion fails.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime date)
        {
            return date.ToString("MM/dd/yyyy");
        }
        return null;
    }

    /// <summary>
    /// Converts a string representation of a date in "MM/dd/yyyy" format back to a DateTime value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>The DateTime value, or null if conversion fails.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string dateString)
        {
            if (DateTime.TryParseExact(dateString, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                // Convert to midnight (00:00:00) of the given day
                return result.Date;
            }
        }
        return null;
    }
}
