using SAE_NICOLASSE.Classe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE_NICOLASSE.UserControls
{
    /// <summary>
    /// Logique d'interaction pour UCDemande.xaml
    /// </summary>
    public partial class UCDemande : UserControl
    {
        private DataAccess dao;

        public UCDemande(ObservableCollection<Demande> demandes, DataAccess dao)
        {
            InitializeComponent();
            this.dao = dao;
            dgDemandes.ItemsSource = demandes;
        }

        private void dgDemandes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Row.DataContext is Demande demandeModifiee)
            {
                try
                {
                    demandeModifiee.Update(this.dao);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Mise à jour impossible. Vérifiez les permissions du rôle.\n" + ex.Message, "Erreur de droits", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
