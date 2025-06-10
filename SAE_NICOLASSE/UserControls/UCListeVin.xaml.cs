using SAE_NICOLASSE.Classe;
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

namespace SAE_NICOLASSE.UserControls
{
    /// <summary>
    /// Logique d'interaction pour UCListeVin.xaml
    /// </summary>
    public partial class UCListeVin : UserControl
    {
        public UCListeVin()
        {
            ChargeData();
            InitializeComponent();
        }
        public void ChargeData()
        {
           /* try
            {
                Vin leVin= new Vin("Pension dog");
                this.DataContext = LaPension;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème lors de récupération des données, 
               
                veuillez consulter votre admin"); 
               
                Application.Current.Shutdown();
            }*/
        }
    }
}
