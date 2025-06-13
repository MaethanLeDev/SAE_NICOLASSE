using SAE_NICOLASSE.Classe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Npgsql;
using SAE_NICOLASSE.Classe;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SAE_NICOLASSE.UserControls
{
    /// <summary>
    /// Logique d'interaction pour UCCreationDemande.xaml
    /// </summary>
    public partial class UCCreationDemande : UserControl, INotifyPropertyChanged
    {
        public event EventHandler DemandeTerminee;
        private DataAccess dao;

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

        public UCCreationDemande(Vin leVin, Employe lEmploye, DataAccess dao)
        {
            InitializeComponent();
            this.LeVin = leVin;
            this.LEmploye = lEmploye;
            this.dao = dao;

            LesClients = new ObservableCollection<Client>();
            ChargerClients();

            this.DataContext = this;
        }

        private void ChargerClients()
        {
            try
            {
                Client clientManager = new Client();
                var tousLesClients = clientManager.FindAll(this.dao);
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
            if (!int.TryParse(txtQuantite.Text, out int quantite) || quantite <= 0)
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
                int idNouvelleDemande = nouvelleDemande.Create(this.dao);
                MessageBox.Show($"La demande a bien été créée avec le numéro {idNouvelleDemande} !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                DemandeTerminee?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors de la création de la demande.\n" + ex.Message, "Erreur Base de Données", MessageBoxButton.OK, MessageBoxImage.Error);
                LogError.Log(ex, "Erreur lors de la création d'une demande");
            }
        }
    }
}
