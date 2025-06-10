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
            LoadVinData();
        }

        private void LoadVinData() // <-- Cette méthode doit être à l'intérieur de la classe MainWindow
        {
            try
            {
                var cmd = new NpgsqlCommand("SELECT * FROM Vin"); // Assure-toi que la table Vin existe
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                VinDataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des vins : " + ex.Message);
            }
        }
    }
}
