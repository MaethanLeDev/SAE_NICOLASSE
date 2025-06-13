// ========================================================================
// FICHIER : UserControls/CreationCommande.xaml.cs
// DÉCISION : Ce fichier vient du projet de ton collègue ("lui").
//            J'ai nettoyé le code, corrigé les références et m'assure
//            qu'il communique correctement avec la MainWindow.
// ========================================================================

using SAE_NICOLASSE.Classe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SAE_NICOLASSE.UserControls
{
    public partial class CreationCommande : UserControl
    {
        private GerantCreationDemande GerantPage { get; set; }
        private Magasin Magasin { get; set; }

        public CreationCommande(Magasin leMagasin)
        {
            InitializeComponent();
            this.Magasin = leMagasin;
            ChargerDemandesAcceptees(leMagasin);
        }

        private void ChargerDemandesAcceptees(Magasin leMagasin)
        {
            GerantPage = new GerantCreationDemande(leMagasin);
            this.DataContext = GerantPage;
        }

        private void AjouterDemande_Click(object sender, RoutedEventArgs e)
        {
            if (!((sender as Button)?.DataContext is Demande demandeAAjouter)) return;

            LigneCommande ligneExistante = GerantPage.VinsDansLaCommande.FirstOrDefault(l => l.UnVin.NumVin == demandeAAjouter.UnVin.NumVin);

            if (ligneExistante != null)
            {
                ligneExistante.FusionnerDemande(demandeAAjouter);
                dgVinsDansLaCommande.Items.Refresh(); // Important pour mettre à jour l'affichage de la quantité
            }
            else
            {
                GerantPage.VinsDansLaCommande.Add(new LigneCommande(demandeAAjouter));
            }

            GerantPage.DemandesDisponibles.Remove(demandeAAjouter);
        }

        private void RetirerDemande_Click(object sender, RoutedEventArgs e)
        {
            if (!((sender as Button)?.DataContext is LigneCommande ligneARetirer)) return;

            GerantPage.VinsDansLaCommande.Remove(ligneARetirer);

            foreach (Demande demandeOriginale in ligneARetirer.DemandesComposees)
            {
                GerantPage.DemandesDisponibles.Add(demandeOriginale);
            }
        }

        private void ValiderCommande_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetrePrincipale = Window.GetWindow(this) as MainWindow;
            if (fenetrePrincipale == null || fenetrePrincipale.MonMagasin == null) return;

            if (GerantPage.VinsDansLaCommande.Count == 0)
            {
                MessageBox.Show("Veuillez ajouter au moins une demande à la commande.", "Commande vide", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Logique pour trouver un employé Admin
            Employe admin = fenetrePrincipale.MonMagasin.LesEmployes.FirstOrDefault(em => em.UnRole.NomRole.Equals("Admin", StringComparison.OrdinalIgnoreCase));
            if (admin == null)
            {
                MessageBox.Show("Aucun employé avec le rôle 'Admin' n'a été trouvé pour valider la commande.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Calcul du prix total
            decimal prixTotal = 0;
            foreach (var ligne in GerantPage.VinsDansLaCommande)
            {
                prixTotal += ligne.UnVin.PrixVin * ligne.QuantiteTotale;
            }

            Commande nouvelleCommande = new Commande(0, admin, DateTime.Now, true, prixTotal);

            try
            {
                int idNouvelleCommande = nouvelleCommande.Create();
                nouvelleCommande.Numcommande = idNouvelleCommande;

                foreach (var ligne in GerantPage.VinsDansLaCommande)
                {
                    // Lier chaque demande originale à la commande nouvellement créée
                    foreach (var demande in ligne.DemandesComposees)
                    {
                        demande.LieAUneCommande(idNouvelleCommande);
                    }

                    // Créer le détail de la commande
                    DetailCommande detail = new DetailCommande(nouvelleCommande, ligne.UnVin, ligne.QuantiteTotale, ligne.UnVin.PrixVin * ligne.QuantiteTotale);
                    detail.Create();
                }

                MessageBox.Show($"Commande n°{idNouvelleCommande} créée avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // Revenir à la liste des commandes
                fenetrePrincipale.ChargeData(); // Recharger toutes les données
                fenetrePrincipale.MainContent.Content = new UCCommande(fenetrePrincipale.MonMagasin.LesCommandes);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors de la création de la commande : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                LogError.Log(ex, "Erreur lors de la validation d'une commande");
            }
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            MainWindow fenetrePrincipale = Window.GetWindow(this) as MainWindow;
            if (fenetrePrincipale != null)
            {
                fenetrePrincipale.MainContent.Content = new UCCommande(fenetrePrincipale.MonMagasin.LesCommandes);
            }
        }
    }
}
