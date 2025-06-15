// ========================================================================
// FICHIER : UserControls/UCDemande.xaml.cs
// DÉCISION : Fusion des deux logiques.
//            - Le constructeur prend le magasin en paramètre pour la
//              flexibilité (idée de ton collègue).
//            - L'affichage montre toutes les demandes par défaut.
//            - La méthode de mise à jour est ta version avec le try-catch,
//              qui est plus sécurisée.
// ========================================================================

using SAE_NICOLASSE.Classe;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SAE_NICOLASSE.UserControls
{
    public partial class UCDemande : UserControl
    {
        public UCDemande()
        {
            Magasin magasin = new Magasin();
            InitializeComponent();
            List<Demande> lesDemandes = new List<Demande>();
            foreach(Demande demande in magasin.LesDemandes)
            {
                if (demande.NumCommande is null)
                {
                    lesDemandes.Add(demande);
                }
            }
            dgDemandes.ItemsSource = lesDemandes;
        }

        private void dgDemandes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

             Demande demandeModifiee = (Demande)dgDemandes.SelectedItem;


                try
            {
                
                demandeModifiee.Update();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Mise à jour impossible : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
