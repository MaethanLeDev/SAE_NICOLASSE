using Npgsql;
using System.Data;
using System.Windows;
using SAE_NICOLASSE.UserControls;

namespace SAE_NICOLASSE
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadVinData();
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