using Npgsql;
using SAE_NICOLASSE.Classe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace SAE_NICOLASSE.Fenêtre
{
    /// <summary>
    /// Logique d'interaction pour FenetreConnexion.xaml
    /// </summary>
    public partial class FenetreConnexion : Window
    {
        private List<Employe> lesemployes;
        private string activeUser = "";
        private string imagePath = "";

        public List<Employe> Lesemployes
        {
            get
            {
                return this.lesemployes;
            }

            set
            {
                this.lesemployes = value;
            }
        }

        public string ActiveUser
        {
            get
            {
                return this.activeUser;
            }

            set
            {
                this.activeUser = value;
            }
        }

        public string ImagePath
        {
            get
            {
                return this.imagePath;
            }

            set
            {
                this.imagePath = value;
            }
        }

        public FenetreConnexion()
        {
            InitializeComponent();
            ChargeUtilisateurs();
            
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            string user = txtUser.Text;
            string mdp = txtMDP.Password;

            foreach (Employe employe in Lesemployes)
            {
                if (user == employe.Login)
                {
                    if (mdp == employe.Mdp)
                    {
                        this.DialogResult = true;
                        ActiveUser = txtUser.Text;
                        
                        ImagePath = $"Fichier/{employe.UnRole.NomRole}.png";
                        




                    }
                }
            }
           
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
        public void ChargeUtilisateurs()
        {
            List<Employe> lesEmploye = new List<Employe>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from employe em join role r on r.numrole=em.numrole;"))
            {
                
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    
                    Role leRole = new Role(
                        Convert.ToInt32(dr["numrole"]),
                        dr["nomrole"].ToString()
                    );

                    
                    Employe lEmploye = new Employe(
                        Convert.ToInt32(dr["numemploye"]),
                        leRole, 
                        dr["nom"].ToString(),
                        dr["prenom"].ToString(),
                        dr["login"].ToString(),
                        dr["mdp"].ToString()
                    );
                    lesEmploye.Add(lEmploye);
                }
            }
            this.Lesemployes  = lesEmploye;
        }



    }
}
