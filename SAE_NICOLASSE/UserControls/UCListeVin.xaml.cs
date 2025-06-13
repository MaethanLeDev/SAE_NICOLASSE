using SAE_NICOLASSE.Classe;
using SAE_NICOLASSE.Fenêtre; // On peut l'enlever si on n'utilise plus de fenêtres
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SAE_NICOLASSE.UserControls
{
    public partial class UCListeVin : UserControl
    {
        public ObservableCollection<Vin> FilteredVins { get; set; }
        public ObservableCollection<string> WineTypes { get; set; }
        public ObservableCollection<string> Appellations { get; set; }
        private readonly List<Vin> _allVins;
        private bool _isResetting = false;

        public UCListeVin(Magasin monMagasin)
        {
            InitializeComponent();
            _allVins = new List<Vin>(monMagasin.LesVins);
            FilteredVins = new ObservableCollection<Vin>(_allVins);

            List<string> tempListTypes = new List<string>();
            foreach (Vin vin in _allVins)
            {
                if (!tempListTypes.Contains(vin.UnType.NomType))
                {
                    tempListTypes.Add(vin.UnType.NomType);
                }
            }
            tempListTypes.Insert(0, "Tous les types");
            WineTypes = new ObservableCollection<string>(tempListTypes);

            List<string> tempListAppellations = new List<string>();
            foreach (Vin vin in _allVins)
            {
                if (!tempListAppellations.Contains(vin.UneAppelation.Nomappelation))
                {
                    tempListAppellations.Add(vin.UneAppelation.Nomappelation);
                }
            }
            tempListAppellations.Insert(0, "Toutes appellations");
            Appellations = new ObservableCollection<string>(tempListAppellations);

            this.DataContext = this;
        }

        private void ApplyFilters()
        {
            List<Vin> updatedList = new List<Vin>(_allVins);
            // ... Ton code de filtre existant ...
            FilteredVins = new ObservableCollection<Vin>(updatedList);
            itemControlVins.ItemsSource = FilteredVins;
        }
        private void Filters_Changed(object sender, RoutedEventArgs e) { if (!_isResetting) ApplyFilters(); }
        private void ResetFilters_Click(object sender, RoutedEventArgs e) { /*...*/ }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Vin selectedVin)
            {
                pnlDetails.DataContext = selectedVin;
                Storyboard sb = (Storyboard)this.Resources["ShowDetailsPanel"];
                sb.Begin();
            }
        }

        private void CloseDetails_Click(object sender, RoutedEventArgs e)
        {
            Storyboard sb = (Storyboard)this.Resources["HideDetailsPanel"];
            sb.Begin();
        }

        private void BtnCreerDemande_Click(object sender, RoutedEventArgs e)
        {
            if (pnlDetails.DataContext is Vin vinSelectionne)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                if (mainWindow.UtilisateurConnecte != null)
                {
                    UCCreationDemande ucCreation = new UCCreationDemande(vinSelectionne, mainWindow.UtilisateurConnecte, mainWindow.Dao);
                    ucCreation.DemandeTerminee += (s, args) =>
                    {
                        mainWindow.ChargeData();
                        mainWindow.MainContent.Content = new UCListeVin(mainWindow.MonMagasin);
                    };
                    mainWindow.MainContent.Content = ucCreation;
                }
            }
        }
    }
}
