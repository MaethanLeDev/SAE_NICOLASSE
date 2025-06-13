// ========================================================================
// FICHIER : Fenêtre/FenetreConnexion.xaml.cs
// DÉCISION : Je conserve ta version ("moi") car elle contient la nouvelle
//            logique de connexion directe qui tente d'établir une
//            connexion avec les identifiants saisis par l'utilisateur.
// ========================================================================

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

            try
            {
                // ÉTAPE 1 : Tenter de créer la connexion avec les identifiants fournis.
                // Si les identifiants sont faux, le constructeur de DataAccess lèvera une exception.
                DataAccess.CreerInstance(txtUser.Text, txtMDP.Password);

                // ÉTAPE 2 : Si la connexion a réussi, on récupère les infos de l'employé.
                // On crée un Employe temporaire juste pour appeler la méthode FindByLogin.
                Employe employeManager = new Employe();
                this.EmployeConnecte = employeManager.FindByLogin(txtUser.Text);

                if (this.EmployeConnecte == null)
                {
                    // Cas très rare : l'utilisateur existe dans la BDD mais pas dans la table EMPLOYE.
                    MessageBox.Show("L'utilisateur est valide mais n'est pas enregistré comme employé.", "Erreur de configuration", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // SUCCÈS !
                    this.ActiveUser = this.EmployeConnecte.Login;
                    this.ImagePath = $"Fichier/{this.EmployeConnecte.UnRole.NomRole}.png";
                    this.DialogResult = true; // On dit à MainWindow que c'est bon.
                }
            }
            catch (Exception ex)
            {
                // L'exception vient probablement du constructeur de DataAccess.
                MessageBox.Show("Échec de la connexion. Vérifiez votre identifiant et mot de passe.", "Erreur d'authentification", MessageBoxButton.OK, MessageBoxImage.Error);
                LogError.Log(ex, "Erreur de connexion directe à la BDD.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
