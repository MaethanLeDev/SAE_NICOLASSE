using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SAE_NICOLASSE.UserControls
{
    public class OuiNonToBoolConverter : IValueConverter
    {
        // Méthode pour convertir la source (string) vers la cible (bool)
        // Convertit "Oui" en true, et toute autre valeur en false.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // On vérifie si la valeur est bien "Oui"
            return value is string str && str.Equals("Oui", StringComparison.OrdinalIgnoreCase);
        }

        // Méthode pour convertir la cible (bool) vers la source (string)
        // Convertit true en "Oui" et false en "Non".
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Si la case est cochée (true), on retourne "Oui", sinon "Non"
            return value is bool b && b ? "Oui" : "Non";
        }
    }
}
