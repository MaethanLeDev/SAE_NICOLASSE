using SAE_NICOLASSE.Classe;
using System;
using System.Windows;

namespace SAE_NICOLASSE.Fenêtre
{
    public partial class FenetreConnexion : Window
    {
        public Employe EmployeConnecte { get; private set; }
        public string ActiveUser { get; private set; }
        public string ImagePath { get; private set; }

        public FenetreConnexion()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtMDP.Password))
            {
                MessageBox.Show("Veuillez entrer un nom d'utilisateur et un mot de passe.", "Champs requis", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // On prépare l'instance avec les identifiants saisi
            DataAccess.CreerInstance(txtUser.Text, txtMDP.Password);

            try
            {
                
                Employe employeManager = new Employe();
                this.EmployeConnecte = employeManager.FindByLogin(txtUser.Text);

                if (this.EmployeConnecte == null)
                {
                    MessageBox.Show("Connexion réussie mais l'utilisateur n'est pas trouvé dans la table Employe.", "Erreur de configuration", MessageBoxButton.OK, MessageBoxImage.Error);
                    DataAccess.ResetInstance(); 
                }
                else
                {
                    // SUCCÈS !
                    this.DialogResult = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Échec de la connexion. Vérifiez votre identifiant et mot de passe.", "Erreur d'authentification", MessageBoxButton.OK, MessageBoxImage.Error);
                
                DataAccess.ResetInstance();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}