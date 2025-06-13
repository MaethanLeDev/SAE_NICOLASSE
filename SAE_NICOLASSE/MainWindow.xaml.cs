using SAE_NICOLASSE;
using SAE_NICOLASSE.Classe;
using SAE_NICOLASSE.Fenêtre;
using SAE_NICOLASSE.UserControls;
using System.ComponentModel;
using System.Windows;

namespace SAE_NICOLASSE
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Magasin monMagasin;
        private string activeUser;
        private string imagePath;

        public DataAccess Dao { get; private set; }
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
                    BoutonDemandes_Click(null, null);
                }
                else
                {
                    BoutonCatalogue_Click(null, null);
                }
            }
        }

        private void AfficherLaFenetreDeConnexion()
        {
            FenetreConnexion loginWindow = new FenetreConnexion();
            if (loginWindow.ShowDialog() != true)
            {
                this.Close();
                return;
            }

            this.UtilisateurConnecte = loginWindow.ActiveEmploye;
            this.ActiveUser = loginWindow.ActiveUser;
            this.ImagePath = loginWindow.ImagePath;

            string username;
            string password;

            if (UtilisateurConnecte.UnRole.NomRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                username = "admin_role";
                password = "motdepasse_admin";
            }
            else
            {
                username = "vendeur_role";
                password = "motdepasse_vendeur";
            }

            string roleConnectionString = $"Host=localhost;Port=5432;Username={username};Password={password};Database=SAE;";
            this.Dao = new DataAccess(roleConnectionString);
        }

        public void ChargeData()
        {
            try
            {
                this.MonMagasin = new Magasin(this.Dao);
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problème lors de la récupération des données pour le rôle '{UtilisateurConnecte.UnRole.NomRole}'.\nVérifiez les permissions (GRANT) dans la base de données.\n\nErreur : {ex.Message}");
                LogError.Log(ex, "Erreur SQL lors du chargement des données par rôle.");
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
            MainContent.Content = new UCDemande(MonMagasin.LesDemandes, this.Dao);
        }

        private void BoutonCommandes_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCCommande(MonMagasin.LesCommandes);
        }
    }
}