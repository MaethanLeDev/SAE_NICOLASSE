using SAE_NICOLASSE.Classe;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace SAE_NICOLASSE.UserControls
{
    public partial class UCCommande : UserControl
    {
        public UCCommande(ObservableCollection<Commande> commandes)
        {
            InitializeComponent();
           this.DataContext = commandes;
        }

        private void CreerCommande_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow == null) return;

            CreationCommande vueCreation = new CreationCommande(mainWindow.MonMagasin);

            mainWindow.MainContent.Content = vueCreation;
        }

        private void SupprimerCommande_Click(object sender, RoutedEventArgs e)
        {
            
            Commande laCommandeASupprimer = (Commande)dgCommande.SelectedItem;
            {
                
                MessageBoxResult resultat = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer la commande n°{laCommandeASupprimer.Numcommande} ?\n\nCette action est irréversible et déliera les demandes associées.",
                    "Confirmation de suppression",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (resultat == MessageBoxResult.Yes)
                {
                    try
                    {
                        
                        int lignesSupprimees = laCommandeASupprimer.Delete();

                        
                        if (lignesSupprimees > 0)
                        {
                            
                            if (this.DataContext is ObservableCollection<Commande> commandes)
                            {
                                commandes.Remove(laCommandeASupprimer);
                                MessageBox.Show("La commande a été supprimée avec succès.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de la suppression : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }



        private void ValiderCheckBox_Click(object sender, RoutedEventArgs e)
        {
            
            Commande commandeModifiee = (Commande)dgCommande.SelectedItem;
            commandeModifiee.Update();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (!(dgCommande.SelectedItem is Commande))
            { return; }
            
            Commande commandeAEditer = (Commande)dgCommande.SelectedItem; 
            

            
            MessageBoxResult resultat = MessageBox.Show(
                $"Voulez-vous éditer la commande n°{commandeAEditer.Numcommande} ?\n\nL'ancienne commande sera annulée et vous serez redirigé pour la recréer.",
                "Confirmation d'édition",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (resultat == MessageBoxResult.Yes)
            {
                try
                {
                    Magasin m = new Magasin();
                    MainWindow fenetrePrincipale = Window.GetWindow(this) as MainWindow;
                    

                    List<Demande> demandesARecreer = new List<Demande>();
                    foreach (Demande d in m.LesDemandes)
                    {
                        if (d.NumCommande != null && d.NumCommande.Numcommande == commandeAEditer.Numcommande)
                        {
                            demandesARecreer.Add(d);
                        }
                    }
                    int lignesSupprimees = commandeAEditer.Delete();

                    if (lignesSupprimees > 0)
                    {
                        m.LesCommandes.Remove(commandeAEditer);

                        
                        foreach (Demande demande in demandesARecreer)
                        {
                            
                            demande.NumCommande = null;
                        }
                        CreationCommande vueCreation = new CreationCommande(m, demandesARecreer);
                        fenetrePrincipale.MainContent.Content = vueCreation;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la préparation de l'édition : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
