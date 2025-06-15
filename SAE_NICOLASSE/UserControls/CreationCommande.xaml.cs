// ========================================================================
// FICHIER : UserControls/CreationCommande.xaml.cs
// DÉCISION : Ce fichier vient du projet de ton collègue ("lui").
//            J'ai nettoyé le code, corrigé les références et m'assure
//            qu'il communique correctement avec la MainWindow.
// ========================================================================

using SAE_NICOLASSE.Classe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SAE_NICOLASSE.UserControls
{
    
    public partial class CreationCommande : UserControl
    {

        // Constructeur modificaiton Commande
        public CreationCommande(Magasin leMagasin, List<Demande> demandesAEditer)
        {
           
            InitializeComponent();
            Magasin magasin = new Magasin();

            
            GerantPage = new GerantCreationDemande(magasin);
            GerantPage.DemandesDisponibles = new ObservableCollection<Demande>();
            foreach (Demande de in magasin.LesDemandes)
            {
                if (de.NumCommande is null)
                GerantPage.DemandesDisponibles.Add(de);
            }
            
            this.DataContext = GerantPage;
            
        }
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
            Demande demandeAAjouter = (Demande)dgDemandesDisponibles.SelectedItem;

            LigneCommande ligneExistante = null;
            foreach(LigneCommande lC in GerantPage.VinsDansLaCommande)
            {
                if (demandeAAjouter.UnVin.NomVin == lC.UnVin.NomVin)
                {
                    ligneExistante = lC;
                    break;
                }
            }

            if (ligneExistante != null)
            {
                MessageBoxResult resultat = MessageBox.Show(
                "Ce vin est déjà dans la commande. Voulez-vous fusionner les quantités ?",
                "Confirmation de Fusion",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

                
                if (resultat == MessageBoxResult.Yes)
                {
                    
                    ligneExistante.FusionnerDemande(demandeAAjouter);
                    dgVinsDansLaCommande.Items.Refresh();
                }
                else
                {
                    
                    GerantPage.VinsDansLaCommande.Add(new LigneCommande(demandeAAjouter));
                }
                

            }
            else
            {
                GerantPage.VinsDansLaCommande.Add(new LigneCommande(demandeAAjouter));
            }

            GerantPage.DemandesDisponibles.Remove(demandeAAjouter);
        }

        private void RetirerDemande_Click(object sender, RoutedEventArgs e)
        {
            LigneCommande ligneARetirer = (LigneCommande)dgVinsDansLaCommande.SelectedItem;

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

            Employe admin = fenetrePrincipale.MonMagasin.LesEmployes.FirstOrDefault(em => em.UnRole.NomRole.Equals("Admin", StringComparison.OrdinalIgnoreCase));
            if (admin == null)
            {
                MessageBox.Show("Aucun employé avec le rôle 'Admin' n'a été trouvé pour valider la commande.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // stockage ligne
            Dictionary<int, LigneCommande> lignesVraimentFusionnees = new Dictionary<int, LigneCommande>();
            foreach (LigneCommande ligneSource in GerantPage.VinsDansLaCommande)
            {

                int idVin = ligneSource.UnVin.NumVin;
                // si le vin pas encore dans le stockage on l'ajoute
                if (!lignesVraimentFusionnees.ContainsKey(idVin))
                {
                    // Si on voit ce vin pour la première fois, on l'ajoute.
                    lignesVraimentFusionnees[idVin] = ligneSource;
                }
                //sinon on le fusionne
                else
                {
                    
                    foreach (Demande demande in ligneSource.DemandesComposees)
                    {
                        lignesVraimentFusionnees[idVin].FusionnerDemande(demande);
                    }
                }
            }
           

            List<LigneCommande> lignesFinalesPourSauvegarde = lignesVraimentFusionnees.Values.ToList();

            //stockage des commandes séparé par fournisseur
            Dictionary<Fournisseur, List<LigneCommande>> commandesParFournisseur = new Dictionary<Fournisseur, List<LigneCommande>>();
            foreach (LigneCommande ligne in lignesFinalesPourSauvegarde)
            {
                // Si le stockage n'a pas encore le fournisseur il l'ajoute
                if (!commandesParFournisseur.ContainsKey(ligne.UnVin.UnFournisseur))
                {
                    commandesParFournisseur[ligne.UnVin.UnFournisseur] = new List<LigneCommande>();
                }
                // Insertion dans la bonne case
                commandesParFournisseur[ligne.UnVin.UnFournisseur].Add(ligne);
            }

            // parcourt le dictionnaire par fournisseur
            foreach (KeyValuePair<Fournisseur, List<LigneCommande>> paire in commandesParFournisseur)
            {
                List<LigneCommande> lignesPourCetteCommande = paire.Value;

                decimal prixTotal = 0;
                foreach (LigneCommande ligne in lignesPourCetteCommande)
                {
                    prixTotal += ligne.UnVin.PrixVin * ligne.QuantiteTotale;
                }

                Commande nouvelleCommande = new Commande(0, admin, DateTime.Now, true, prixTotal);

                try
                {
                    int idNouvelleCommande = nouvelleCommande.Create();
                    nouvelleCommande.Numcommande = idNouvelleCommande;

                    foreach (LigneCommande ligne in lignesPourCetteCommande)
                    {
                        foreach (Demande demande in ligne.DemandesComposees)
                        {
                            demande.LieAUneCommande(idNouvelleCommande);
                            fenetrePrincipale.MonMagasin.LesDemandes.Remove(demande);
                        }

                        DetailCommande detail = new DetailCommande(nouvelleCommande, ligne.UnVin, ligne.QuantiteTotale, ligne.UnVin.PrixVin * ligne.QuantiteTotale);
                        detail.Create();
                    }

                    fenetrePrincipale.MonMagasin.LesCommandes.Add(nouvelleCommande);
                    MessageBox.Show($"Commande n°{idNouvelleCommande} pour le fournisseur '{paire.Key.NomFournisseur}' créée avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur est survenue pour le fournisseur '{paire.Key.NomFournisseur}' : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            
            fenetrePrincipale.MainContent.Content = new UCCommande(fenetrePrincipale.MonMagasin.LesCommandes);
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
