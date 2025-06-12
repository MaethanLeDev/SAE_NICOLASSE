using Npgsql;
using System.Data;
using System.Windows;
using SAE_NICOLASSE.UserControls;
using SAE_NICOLASSE.Fenêtre;
using SAE_NICOLASSE.Classe;
using System;

namespace SAE_NICOLASSE
{
    public partial class MainWindow : Window
    {
        private Magasin monMagasin;
        private string activeUser = ""; // Ajout de la propriété ActiveUser
        private string imagePath = ""; // Ajout de la propriété ImagePath

        public MainWindow()
        {
            ChargeData();
            InitializeComponent();
            AfficherLaFenetreDeConnexion();
            MonMagasin = new Magasin();

            // Définir le DataContext sur cette instance de MainWindow
            this.DataContext = this;

            BoutonCatalogue_Click(null, null); // Par défaut, affiche la liste des vins dans le catalogue
        }

        public Magasin MonMagasin
        {
            get { return this.monMagasin; }
            set { this.monMagasin = value; }
        }

        // Propriété ActiveUser dans MainWindow
        public string ActiveUser
        {
            get { return this.activeUser; }
            set { this.activeUser = value; }
        }

        // Propriété ImagePath dans MainWindow
        public string ImagePath
        {
            get { return this.imagePath; }
            set { this.imagePath = value; }
        }

        private void AfficherLaFenetreDeConnexion()
        {
            FenetreConnexion loginWindow = new FenetreConnexion();
            //Attend que la fenêtre de connexion soit fermée
            bool? resultat = loginWindow.ShowDialog();

            if (resultat != true)
            {
                this.Close();
            }
            else
            {
                // Récupérer l'utilisateur connecté et son image
                this.ActiveUser = loginWindow.ActiveUser;
                this.ImagePath = loginWindow.ImagePath;
            }
        }

        // Gérer les clics sur les boutons de navigation
        private void BoutonCatalogue_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCListeVin(MonMagasin);
        }

        private void BoutonDemandes_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCDemande();
        }

        private void BoutonCommandes_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCCommande();
        }
        public void ChargeData()
        {
            try
            {
                Magasin monMagasin = new Magasin();
                this.DataContext = monMagasin;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème lors de récupération des données,veuillez consulter votre admin");
                LogError.Log(ex, "Erreur SQL");
                Application.Current.Shutdown();
            }
        }
    }
}