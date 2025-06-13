using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows;
using SAE_NICOLASSE.Classe;

namespace SAE_NICOLASSE.UserControls
{
    public partial class UCCommande : UserControl
    {
        public UCCommande(ObservableCollection<Commande> commandes)
        {
            InitializeComponent();
            dataGridCommandes.ItemsSource = commandes;
        }
    }
}
