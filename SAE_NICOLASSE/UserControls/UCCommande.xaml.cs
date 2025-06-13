// ========================================================================
// FICHIER : UserControls/UCCommande.xaml.cs
// DÉCISION : Basé sur la version de ton collègue ("lui"), qui contient
//            toute la logique métier. J'ai simplifié et clarifié le code
//            pour qu'il s'intègre parfaitement à notre structure finale.
// ========================================================================

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
            // Le DataContext de ce UserControl est directement la liste des commandes.
            // Le DataGrid va automatiquement utiliser cette source.
            this.DataContext = commandes;
        }

        private void CreerCommande_Click(object sender, RoutedEventArgs e)
        {
            // 1. On récupère la MainWindow pour accéder à ses propriétés
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow == null) return;

            // 2. On crée une instance du UserControl de création de commande
            CreationCommande vueCreation = new CreationCommande(mainWindow.MonMagasin);

            // 3. On remplace le contenu actuel de la MainWindow par ce nouveau UserControl
            mainWindow.MainContent.Content = vueCreation;
        }

        private void SupprimerCommande_Click(object sender, RoutedEventArgs e)
        {
            // On récupère l'objet Commande associé à la ligne du bouton cliqué
            if ((sender as Button)?.DataContext is Commande laCommandeASupprimer)
            {
                // 1. Demander confirmation à l'utilisateur
                MessageBoxResult resultat = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer la commande n°{laCommandeASupprimer.Numcommande} ?\n\nCette action est irréversible et déliera les demandes associées.",
                    "Confirmation de suppression",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (resultat == MessageBoxResult.Yes)
                {
                    try
                    {
                        // 2. Appeler la méthode Delete pour la supprimer de la base de données.
                        int lignesSupprimees = laCommandeASupprimer.Delete();

                        // 3. Si la suppression en BDD a réussi, on la retire de la liste en mémoire.
                        if (lignesSupprimees > 0)
                        {
                            // On récupère la liste des commandes directement depuis le DataContext et on supprime l'élément.
                            // L'interface se mettra à jour automatiquement grâce à l'ObservableCollection.
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
    }
}
