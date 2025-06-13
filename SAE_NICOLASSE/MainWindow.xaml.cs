// ========================================================================
// FICHIER : MainWindow.xaml.cs
// DÉCISION : Je conserve ta version ("moi"), car c'est la seule qui
//            contient toute la logique de connexion, de gestion des rôles
//            et de navigation entre les différents écrans.
// ========================================================================

using SAE_NICOLASSE.Classe;
using SAE_NICOLASSE.Fenêtre;
using SAE_NICOLASSE.UserControls;
using System;
using System.ComponentModel;
using System.Windows;

namespace SAE_NICOLASSE
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Magasin monMagasin;
        private string activeUser;
        private string imagePath;

        public Employe UtilisateurConnecte { get; private set; }

        public string ActiveUser
        {
            get => activeUser;
            set { activeUser = value; OnPropertyChanged(nameof(ActiveUser)); }
        }
        public string ImagePath
        {
            get => imagePath;
            set { imagePath = value; OnPropertyChanged(nameof(ImagePath)); }
        }

        public Magasin MonMagasin
        {
            get => monMagasin;
            set { monMagasin = value; OnPropertyChanged(nameof(MonMagasin)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();

            AfficherLaFenetreDeConnexion();

            if (this.UtilisateurConnecte != null)
            {
                ChargeData();
                ConfigurerInterfaceSelonRole();

                if (UtilisateurConnecte.UnRole.NomRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    // L'admin voit les commandes en premier
                    BoutonCommandes_Click(null, null);
                }
                else
                {
                    // Le vendeur voit le catalogue en premier
                    BoutonCatalogue_Click(null, null);
                }
            }
            else
            {
                // Si la connexion a échoué, on ferme l'application
                this.Close();
            }
        }

        private void AfficherLaFenetreDeConnexion()
        {
            FenetreConnexion loginWindow = new FenetreConnexion();
            if (loginWindow.ShowDialog() == true)
            {
                // La connexion a déjà été établie, on récupère juste les infos
                this.UtilisateurConnecte = loginWindow.EmployeConnecte;
                this.ActiveUser = loginWindow.ActiveUser;
                this.ImagePath = loginWindow.ImagePath;
            }
            else
            {
                // Si l'utilisateur a annulé, on met l'employé à null pour fermer l'appli
                this.UtilisateurConnecte = null;
            }
        }

        public void ChargeData()
        {
            try
            {
                this.MonMagasin = new Magasin();
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problème lors de la récupération des données : {ex.Message}");
                this.Close();
            }
        }

        private void ConfigurerInterfaceSelonRole()
        {
            string role = UtilisateurConnecte.UnRole.NomRole;

            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                BoutonCatalogue.Visibility = Visibility.Collapsed;
                BoutonCommandes.Visibility = Visibility.Visible;
                BoutonDemandes.Visibility = Visibility.Visible;
            }
            else if (role.Equals("Vendeur", StringComparison.OrdinalIgnoreCase))
            {
                BoutonCatalogue.Visibility = Visibility.Visible;
                BoutonCommandes.Visibility = Visibility.Collapsed;
                BoutonDemandes.Visibility = Visibility.Visible;
            }
            else
            {
                // Par sécurité, si le rôle n'est pas reconnu
                BoutonCatalogue.Visibility = Visibility.Collapsed;
                BoutonCommandes.Visibility = Visibility.Collapsed;
                BoutonDemandes.Visibility = Visibility.Collapsed;
            }
        }

        private void BoutonCatalogue_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCListeVin(MonMagasin);
        }

        private void BoutonDemandes_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCDemande(MonMagasin);
        }

        private void BoutonCommandes_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCCommande(MonMagasin.LesCommandes);
        }
    }
}
