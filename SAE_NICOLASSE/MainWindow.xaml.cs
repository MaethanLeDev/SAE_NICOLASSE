using Npgsql;
using System.Data;
using System.Windows;
using SAE_NICOLASSE.UserControls;
using SAE_NICOLASSE.Fenêtre;
using SAE_NICOLASSE.Classe;
using System;
using System.ComponentModel; // Ajout nécessaire

namespace SAE_NICOLASSE
{
    public partial class MainWindow : Window, INotifyPropertyChanged // Implémentation de INotifyPropertyChanged
    {
        private Magasin monMagasin;
        private string activeUser = ""; // Variable privée pour ActiveUser
        private string imagePath = ""; // Variable privée pour ImagePath

        // Événement pour notifier les changements de propriétés
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            ChargeData();
            AfficherLaFenetreDeConnexion();
            BoutonCatalogue_Click(null, null); // Par défaut, affiche la liste des vins dans le catalogue
        }

        public Magasin MonMagasin
        {
            get { return this.monMagasin; }
            set { this.monMagasin = value; }
        }

        // Propriété ActiveUser avec notification de changement
        public string ActiveUser
        {
            get { return this.activeUser; }
            set
            {
                if (this.activeUser != value)
                {
                    this.activeUser = value;
                    OnPropertyChanged(nameof(ActiveUser)); // Notification du changement
                }
            }
        }

        // Propriété ImagePath avec notification de changement
        public string ImagePath
        {
            get { return this.imagePath; }
            set
            {
                if (this.imagePath != value)
                {
                    this.imagePath = value;
                    OnPropertyChanged(nameof(ImagePath)); // Notification du changement
                }
            }
        }

        // Méthode pour déclencher l'événement PropertyChanged
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                Console.WriteLine($"C'est bon : {this.ActiveUser}, {this.ImagePath}");
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
                this.monMagasin = monMagasin;
                this.DataContext = this;
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