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
        public Magasin MonMagasin { get; private set; }
        public Employe UtilisateurConnecte { get; private set; }
        public string ActiveUser { get; set; }
        public string ImagePath { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            AfficherLaFenetreDeConnexion();
        }

       
        public MainWindow(Employe employeConnecte)
        {
            InitializeComponent();
            // On initialise directement la session avec l'employé fourni.
            InitialiserSession(employeConnecte);
        }

        private void AfficherLaFenetreDeConnexion()
        {
            FenetreConnexion loginWindow = new FenetreConnexion();
            if (loginWindow.ShowDialog() == true)
            {
                // La connexion a réussi, on initialise la session.
                InitialiserSession(loginWindow.EmployeConnecte);
            }
            else
            {
                // L'utilisateur a annulé, on ferme l'application.
                Application.Current.Shutdown();
            }
        }

        // Méthode pour initialiser ou réinitialiser la session de l'utilisateur.
        private void InitialiserSession(Employe employe)
        {
            this.UtilisateurConnecte = employe;
            this.ActiveUser = this.UtilisateurConnecte.Login;
            this.ImagePath = $"Fichier/{this.UtilisateurConnecte.UnRole.NomRole}.png";
            this.DataContext = this;

            ChargeData();
            ConfigurerInterfaceSelonRole();

            // Affiche la vue par défaut en fonction du rôle
            if (UtilisateurConnecte.UnRole.NomRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                BoutonCommandes_Click(null, null);
            }
            else
            {
                BoutonCatalogue_Click(null, null);
            }
        }

        private void Deconnexion_Click(object sender, RoutedEventArgs e)
        {
            // 1. On oublie la connexion de l'ancien utilisateur.
            DataAccess.ResetInstance();

            // 2. On cache la fenêtre actuelle.
            this.Hide();

            // 3. On affiche la fenêtre de connexion.
            FenetreConnexion loginWindow = new FenetreConnexion();
            if (loginWindow.ShowDialog() == true)
            {
                // 4. Si la connexion réussit, on crée une nouvelle MainWindow
                //    en utilisant le NOUVEAU constructeur qui ne redemande pas le login.
                MainWindow nouvelleFenetre = new MainWindow(loginWindow.EmployeConnecte);
                nouvelleFenetre.Show();
            }

            // 5. Quoiqu'il arrive (connexion réussie ou annulée), on ferme l'ancienne fenêtre.
            this.Close();
        }

        
        public void ChargeData()
        {
            try
            {
                this.MonMagasin = new Magasin();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problème lors de la récupération des données : {ex.Message}");
            }
        }

        private void ConfigurerInterfaceSelonRole()
        {
            // On récupère le nom du rôle de l'utilisateur qui est connecté.
            string role = UtilisateurConnecte.UnRole.NomRole;

            // On utilise StringComparison.OrdinalIgnoreCase pour ignorer les majuscules/minuscules (plus sûr).
            if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // Si c'est un Admin, on affiche les boutons Commandes et Demandes.
                BoutonCatalogue.Visibility = Visibility.Collapsed;
                BoutonCommandes.Visibility = Visibility.Visible;
                BoutonDemandes.Visibility = Visibility.Visible;
            }
            else if (role.Equals("Vendeur", StringComparison.OrdinalIgnoreCase))
            {
                // Si c'est un Vendeur, on affiche les boutons Catalogue et Demandes.
                BoutonCatalogue.Visibility = Visibility.Visible;
                BoutonCommandes.Visibility = Visibility.Collapsed;
                BoutonDemandes.Visibility = Visibility.Visible;
            }
            else
            {
                // Par sécurité, si le rôle n'est ni l'un ni l'autre, on cache tout.
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
            MainContent.Content = new UCDemande();
        }

        private void BoutonCommandes_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCCommande(MonMagasin.LesCommandes);
        }
    }
}