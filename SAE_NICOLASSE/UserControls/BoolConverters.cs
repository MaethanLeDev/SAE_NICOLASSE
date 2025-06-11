using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SAE_NICOLASSE.UserControls
{
    // Convertisseur pour transformer un bool en couleur de badge
    public class BoolToColorConverter : IValueConverter
    {
        public static readonly BoolToColorConverter Instance = new BoolToColorConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isValidated)
            {
                return isValidated
                    ? new SolidColorBrush(Color.FromRgb(34, 197, 94))  // Vert pour validé
                    : new SolidColorBrush(Color.FromRgb(249, 115, 22)); // Orange pour en attente
            }
            return new SolidColorBrush(Color.FromRgb(156, 163, 175)); // Gris par défaut
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Convertisseur pour transformer un bool en texte de statut
    public class BoolToStatusConverter : IValueConverter
    {
        public static readonly BoolToStatusConverter Instance = new BoolToStatusConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isValidated)
            {
                return isValidated ? "Validated" : "Pending";
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}