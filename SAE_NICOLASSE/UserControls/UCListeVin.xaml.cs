using SAE_NICOLASSE.Classe;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation; // <-- Assure-toi que cette ligne est bien présente !


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

            // ... (ton code pour remplir les ComboBox reste ici) ...
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

        // ... (tes méthodes ApplyFilters, Filters_Changed et ResetFilters_Click restent ici) ...
        private void ApplyFilters()
        {
            List<Vin> updatedList = new List<Vin>();
            foreach (Vin vin in _allVins)
            {
                bool keepThisVin = true;
                if (!string.IsNullOrWhiteSpace(txtRecherche.Text))
                {
                    string searchText = txtRecherche.Text.ToLower();
                    if (!vin.NomVin.ToLower().Contains(searchText) && !vin.Descriptif.ToLower().Contains(searchText))
                    {
                        keepThisVin = false;
                    }
                }
                if (cmbTypeVin.SelectedItem is string selectedType && selectedType != "Tous les types")
                {
                    if (vin.UnType.NomType != selectedType)
                    {
                        keepThisVin = false;
                    }
                }
                if (cmbAppellation.SelectedItem is string selectedAppellation && selectedAppellation != "Toutes appellations")
                {
                    if (vin.UneAppelation.Nomappelation != selectedAppellation)
                    {
                        keepThisVin = false;
                    }
                }
                if (int.TryParse(txtAnnee.Text, out int annee) && annee > 0)
                {
                    if (vin.Millesime != annee)
                    {
                        keepThisVin = false;
                    }
                }
                if (decimal.TryParse(txtPrixMax.Text, out decimal prixMax) && prixMax > 0)
                {
                    if (vin.PrixVin > prixMax)
                    {
                        keepThisVin = false;
                    }
                }
                if (keepThisVin)
                {
                    updatedList.Add(vin);
                }
            }

            FilteredVins.Clear();
            foreach (var vin in updatedList)
            {
                FilteredVins.Add(vin);
            }
        }
        private void Filters_Changed(object sender, RoutedEventArgs e)
        {
            if (_isResetting) return;
            ApplyFilters();
        }
        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            _isResetting = true;
            txtRecherche.Text = string.Empty;
            cmbTypeVin.SelectedIndex = 0;
            cmbAppellation.SelectedIndex = 0;
            txtAnnee.Text = string.Empty;
            txtPrixMax.Text = string.Empty;
            _isResetting = false;
            ApplyFilters();
        }


        // ===============================================================
        // DÉBUT : CODE À AJOUTER POUR LE PANNEAU DE DÉTAILS
        // ===============================================================

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Récupérer le vin associé au bouton qui a été cliqué
            if (sender is Button button && button.DataContext is Vin selectedVin)
            {
                // 2. Définir le DataContext du panneau de détails. 
                //    Le XAML du panneau va automatiquement utiliser ce vin pour afficher les infos.
                pnlDetails.DataContext = selectedVin;

                // 3. Récupérer et démarrer l'animation pour faire apparaître le panneau
                Storyboard sb = (Storyboard)this.Resources["ShowDetailsPanel"];
                sb.Begin();
            }
        }

        private void CloseDetails_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer et démarrer l'animation pour faire disparaître le panneau
            Storyboard sb = (Storyboard)this.Resources["HideDetailsPanel"];
            sb.Begin();
        }

        // ===============================================================
        // FIN : CODE À AJOUTER POUR LE PANNEAU DE DÉTAILS
        // ===============================================================
    }
}