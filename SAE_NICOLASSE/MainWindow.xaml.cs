using Npgsql;
using System.Data;
using System.Windows;
using SAE_NICOLASSE.UserControls;
using SAE_NICOLASSE.Fenêtre;

namespace SAE_NICOLASSE
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AfficherLaFenetreDeConnexion();
        }

        private void LoadVinData()
        {
            try
            {
                var cmd = new NpgsqlCommand("SELECT * FROM Vin");
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);

                // Ici vous pouvez utiliser dt pour alimenter votre ItemsControl
                // WineItemsControl.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des vins : " + ex.Message);
            }
        /* private void LoadVinData() // <-- Cette méthode doit être à l'intérieur de la classe MainWindow
         {
             try
             {
                 var cmd = new NpgsqlCommand("SELECT * FROM Vin"); // Assure-toi que la table Vin existe
                 DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);

             }
             catch (Exception ex)
             {
                 MessageBox.Show("Erreur lors du chargement des vins : " + ex.Message);
             }
         }*/
        private void AfficherLaFenetreDeConnexion()
        {
           
            FenetreConnexion loginWindow = new FenetreConnexion();

            //Attend que la fenêtre de connexion soit fermée
            bool? resultat = loginWindow.ShowDialog();

            
            if (resultat != true)
            {
                this.Close();
            }
            // Si la connexion réussit (resultat est 'true'), la méthode se termine
            // simplement. Le constructeur finit son travail, et la MainWindow 
            // reste affichée et devient utilisable.
        }

        // Gérer les clics sur les boutons de navigation
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCListeVin();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCDemande();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UCCommande();
        }
    }
}