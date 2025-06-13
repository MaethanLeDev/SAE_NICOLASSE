

using System;
using System.Globalization;
using System.Windows.Data;

namespace SAE_NICOLASSE.Classe
{
    public class BoolToOuiNonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                return val ? "Oui" : "Non";
            }
            return "Non";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string str && str.Equals("Oui", StringComparison.OrdinalIgnoreCase);
        }
    }
}
