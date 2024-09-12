using OwlReadingRoom.ViewModels;
using System.Collections.ObjectModel;
using System.Globalization;

namespace OwlReadingRoom.Utils
{
    /// <summary>
    /// Retrieves the property value for the selected desk from the list of provided desks.
    /// </summary>
    public class NameToDeskInfoConverter : IValueConverter
    {
        /// <summary>
        /// Converts the desk list to fetch property value for the selected desk.
        /// </summary>
        /// <param name="value">The list of desk info view model.</param>
        /// <param name="targetType">The type of object where the converter function is triggered.</param>
        /// <param name="parameter">The parameters responsible to determine the property value.Eg: N1,Status</param>
        /// <param name="culture">the culture info for this particular information.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is List<DeskInfoViewModel> desks) || !(parameter is string paramString))
                return null;

            var parameters = paramString.Split(',');
            if (parameters.Length != 2)
                return null;

            var frameName = parameters[0];
            var propertyName = parameters[1];

            var desk = desks.FirstOrDefault(d => d.Name == frameName);
            if (desk == null)
                return null;

            var property = desk.GetType().GetProperty(propertyName);
            return property?.GetValue(desk);
        }

        /// <summary>
        /// Converts a value from the binding target back to the binding source.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <exception cref="NotImplementedException">Throws if invoked since this function is not usable.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
