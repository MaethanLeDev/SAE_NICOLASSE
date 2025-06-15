// ========================================================================
// FICHIER : UserControls/UCCreationDemande.xaml.cs
// DÉCISION : Ce fichier vient de ta version ("moi"). Il est la logique
//            derrière la création d'une demande simple et est essentiel
//            au workflow du vendeur.
// ========================================================================

using SAE_NICOLASSE.Classe;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SAE_NICOLASSE.UserControls
{
    public partial class UCCreationDemande : UserControl, INotifyPropertyChanged
    {

        public event EventHandler DemandeTerminee;

        public Vin LeVin { get; set; }
        public Employe LEmploye { get; set; }
        public ObservableCollection<Client> LesClients { get; set; }

        private Client _clientSelectionne;
        public Client ClientSelectionne
        {
            get => _clientSelectionne;
            set
            {
                _clientSelectionne = value;
                OnPropertyChanged(nameof(ClientSelectionne));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UCCreationDemande(Vin leVin, Employe lEmploye)
        {
            InitializeComponent();
            this.LeVin = leVin;
            this.LEmploye = lEmploye;

            LesClients = new ObservableCollection<Client>();
            ChargerClients();

            this.DataContext = this;
        }

        private void ChargerClients()
        {
            try
            {
                Client clientManager = new Client();
                var tousLesClients = clientManager.FindAll();
                LesClients.Clear();
                foreach (var client in tousLesClients)
                {
                    LesClients.Add(client);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des clients: " + ex.Message);
            }
        }

        private void CalculerPrixTotal()
        {
            if (int.TryParse(txtQuantite.Text, out int quantite) && quantite > 0)
            {
                decimal prixTotal = LeVin.PrixVin * quantite;
                tbPrixTotal.Text = prixTotal.ToString("C", new System.Globalization.CultureInfo("fr-FR"));
            }
            else
            {
                tbPrixTotal.Text = "0,00 €";
            }
        }

        private void TxtQuantite_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculerPrixTotal();
        }

        private void DgClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgClients.SelectedItem is Client client)
            {
                this.ClientSelectionne = client;
            }
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            DemandeTerminee?.Invoke(this, EventArgs.Empty);
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtQuantite.Text, out int quantite) || quantite <= 0 || quantite >= 100)
            {
                MessageBox.Show("Veuillez entrer une quantité valide.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ClientSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un client.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Demande nouvelleDemande = new Demande();
            nouvelleDemande.UnVin = this.LeVin;
            nouvelleDemande.UnEmploye = this.LEmploye;
            nouvelleDemande.NumClient = this.ClientSelectionne;
            nouvelleDemande.QuantiteDemande = quantite;
            nouvelleDemande.DateDemande = DateTime.Now;
            nouvelleDemande.Accepter = "En attente";
            nouvelleDemande.NumCommande = null;

            try
            {
                int idNouvelleDemande = nouvelleDemande.Create();
                MessageBox.Show($"La demande a bien été créée avec le numéro {idNouvelleDemande} !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                DemandeTerminee?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors de la création de la demande.\n" + ex.Message, "Erreur Base de Données", MessageBoxButton.OK, MessageBoxImage.Error);
                LogError.Log(ex, "Erreur lors de la création d'une demande");
            }
        }

        private void CreerNouveauClient(object sender, RoutedEventArgs e)
        {
            // 1. On vérifie que les champs nécessaires ne sont pas vides
            if (string.IsNullOrWhiteSpace(txtNouveauNom.Text) || string.IsNullOrWhiteSpace(txtNouveauPrenom.Text))
            {
                MessageBox.Show("Veuillez entrer au moins un nom et un prénom pour le nouveau client.", "Champs requis", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2. On crée un nouvel objet Client avec les informations saisies
            Client nouveauClient = new Client
            {
                NomClient = txtNouveauNom.Text,
                PrenomClient = txtNouveauPrenom.Text,
                MailClient = txtNouveauMail.Text
            };

            try
            {
                // 3. On appelle sa méthode Create() pour l'insérer en BDD et récupérer son ID
                int newId = nouveauClient.Create();
                nouveauClient.NumClient = newId; // On met à jour l'objet avec son nouvel ID

                // 4. On l'ajoute à la liste affichée dans le DataGrid
                this.LesClients.Add(nouveauClient);

                // 5. On le sélectionne automatiquement
                this.ClientSelectionne = nouveauClient;
                dgClients.SelectedItem = nouveauClient;
                dgClients.ScrollIntoView(nouveauClient);

                // 6. On vide les champs de saisie
                txtNouveauNom.Text = "";
                txtNouveauPrenom.Text = "";
                txtNouveauMail.Text = "";

                MessageBox.Show("Nouveau client créé et sélectionné avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors de la création du client : " + ex.Message);
                LogError.Log(ex, "Erreur création client");
            }
        }
    }
}
