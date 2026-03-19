using System.Globalization;
using System.Windows.Data;

namespace TheMagicOfPerfumes.Helpers;

public class DecimalInputConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null) return string.Empty;
        if (value is decimal d) return d == 0 ? string.Empty : d.ToString("0.##", CultureInfo.InvariantCulture);
        return string.Empty;
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var input = value?.ToString()?.Trim().Replace(",", ".");
        if (string.IsNullOrWhiteSpace(input)) return null;
        if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;
        return null; // invalid input → null → CanSave blocks saving
    }
}