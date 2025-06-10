using Npgsql;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SAE_NICOLASSE.UserControls;
using SAE_NICOLASSE.Fenêtre;



namespace SAE_NICOLASSE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AfficherLaFenetreDeConnexion();
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
