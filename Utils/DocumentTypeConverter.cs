using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Constants;
using System.Globalization;

namespace OwlReadingRoom.Utils;

public class DocumentTypeConverter : IValueConverter
{
    /// <summary>
    /// Converts a DocumentType enum value to its string representation.
    /// </summary>
    /// <param name="value">The DocumentType enum value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>The string representation of the DocumentType.</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        DocumentType documentType = (DocumentType)value;
        return documentType switch
        {
            DocumentType.CITIZENSHIP => AppConstants.DocumentConstants.citizenship,
            DocumentType.LICENSE => AppConstants.DocumentConstants.license,
            DocumentType.VOTERS_ID => AppConstants.DocumentConstants.votersId,
            DocumentType.PASSPORT => AppConstants.DocumentConstants.passport,
        };
    }

    /// <summary>
    /// Converts a string representation of a document type back to a DocumentType enum value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>The DocumentType enum value.</returns>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string documentTypeString = value as string;
        return documentTypeString switch
        {
            AppConstants.DocumentConstants.citizenship => DocumentType.CITIZENSHIP,
            AppConstants.DocumentConstants.license => DocumentType.LICENSE,
            AppConstants.DocumentConstants.votersId => DocumentType.VOTERS_ID,
            AppConstants.DocumentConstants.passport => AppConstants.DocumentConstants.passport
        };

    }
}
