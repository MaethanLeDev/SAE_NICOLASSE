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
        public UCDemande(Magasin leMagasin)
        {
            InitializeComponent();

            // On affiche par défaut toutes les demandes.
            // Le filtrage se fera dans d'autres écrans si nécessaire.
            dgDemandes.ItemsSource = leMagasin.LesDemandes;
        }

        private void dgDemandes_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            // On s'assure que l'objet édité est bien une Demande
            if (e.Row.DataContext is Demande demandeModifiee)
            {
                try
                {
                    // On appelle la méthode Update de l'objet Demande pour
                    // sauvegarder le changement de statut dans la base de données.
                    demandeModifiee.Update();
                }
                catch (Exception ex)
                {
                    // Si une erreur survient (ex: droits insuffisants), on prévient l'utilisateur.
                    MessageBox.Show("Mise à jour impossible : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
