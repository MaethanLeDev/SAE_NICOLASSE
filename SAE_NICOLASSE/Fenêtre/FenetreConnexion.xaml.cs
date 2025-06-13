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

        public Employe ActiveEmploye { get; private set; }
        public string ActiveUser { get; private set; }
        public string ImagePath { get; private set; }

        public FenetreConnexion()
        {
            InitializeComponent();
            ChargeUtilisateurs();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string user = txtUser.Text;
            string mdp = txtMDP.Password;

            foreach (Employe employe in lesemployes)
            {
                if (user == employe.Login && mdp == employe.Mdp)
                {
                    this.ActiveEmploye = employe;
                    this.ActiveUser = employe.Login;
                    this.ImagePath = $"Fichier/{employe.UnRole.NomRole}.png";
                    this.DialogResult = true;
                    return;
                }
            }
            MessageBox.Show("Identifiant ou mot de passe incorrect.", "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public void ChargeUtilisateurs()
        {
            // ATTENTION : Cette chaîne de connexion est celle de l'utilisateur "maître" (ex: postgres).
            // Elle est utilisée UNIQUEMENT dans cette fenêtre pour vérifier le mot de passe.
            string connectionStringMaitre = "Host=localhost;Port=5432;Username=postgres;Password=r9T10jzEfwqnwd2;Database=SAE;";
            DataAccess daoMaitre = new DataAccess(connectionStringMaitre);

            lesemployes = new List<Employe>();
            string sql = "SELECT * FROM employe em JOIN role r ON r.numrole = em.numrole;";
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
            {
                try
                {
                    DataTable dt = daoMaitre.ExecuteSelect(cmdSelect);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Role leRole = new Role(Convert.ToInt32(dr["numrole"]), dr["nomrole"].ToString());
                        Employe lEmploye = new Employe(Convert.ToInt32(dr["numemploye"]), leRole, dr["nom"].ToString(), dr["prenom"].ToString(), dr["login"].ToString(), dr["mdp"].ToString());
                        lesemployes.Add(lEmploye);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Impossible de vérifier les utilisateurs. L'application va se fermer.\n" + ex.Message);
                    LogError.Log(ex, "Erreur critique dans ChargeUtilisateurs");
                    this.Close();
                }
            }
        }
    }
}
