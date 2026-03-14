using System.Globalization;
using System.Windows.Data;

namespace TheMagicOfPerfumes.Helpers;

public class BoolToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var parts = parameter?.ToString()?.Split('|');
        if (parts?.Length == 2)
            return value is true ? parts[0] : parts[1];
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}