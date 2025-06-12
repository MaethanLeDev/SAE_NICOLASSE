using SAE_NICOLASSE.Classe;
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

        // Champ privé pour stocker la liste originale et complète de tous les vins.
        // Le '_' indique que c'est une variable membre de la classe.
        private readonly List<Vin> _allVins;

        // Champ privé utilisé comme un "drapeau" pour la fonction de réinitialisation.
        private bool _isResetting = false;

        public UCListeVin(Magasin monMagasin)
        {
            InitializeComponent();
            _allVins = new List<Vin>(monMagasin.LesVins);
            FilteredVins = new ObservableCollection<Vin>(_allVins);

            // --- Remplissage des ComboBox SANS LINQ ---

            // 1. Pour les types de vin
            List<string> tempListTypes = new List<string>();
            foreach (Vin vin in _allVins)
            {
                // Si la liste temporaire ne contient pas déjà ce type de vin...
                if (!tempListTypes.Contains(vin.UnType.NomType))
                {
                    // ... on l'ajoute. Cela remplace la méthode .Distinct()
                    tempListTypes.Add(vin.UnType.NomType);
                }
            }
            tempListTypes.Insert(0, "Tous les types"); // On ajoute l'option pour tout voir
            WineTypes = new ObservableCollection<string>(tempListTypes);


            // 2. Pour les appellations
            List<string> tempListAppellations = new List<string>();
            foreach (Vin vin in _allVins)
            {
                // Même logique que pour les types
                if (!tempListAppellations.Contains(vin.UneAppelation.Nomappelation))
                {
                    tempListAppellations.Add(vin.UneAppelation.Nomappelation);
                }
            }
            tempListAppellations.Insert(0, "Toutes appellations");
            Appellations = new ObservableCollection<string>(tempListAppellations);

            this.DataContext = this;
        }

        /// <summary>
        /// Méthode qui applique les filtres, version SANS LINQ (.Where)
        /// </summary>
        private void ApplyFilters()
        {
            // On crée une nouvelle liste vide qui contiendra nos résultats filtrés
            List<Vin> updatedList = new List<Vin>();

            // On parcourt CHAQUE vin de la liste originale complète
            foreach (Vin vin in _allVins)
            {
                bool keepThisVin = true; // On part du principe qu'on garde le vin...

                // --- TEST 1 : Le texte de recherche ---
                if (!string.IsNullOrWhiteSpace(txtRecherche.Text))
                {
                    string searchText = txtRecherche.Text.ToLower();
                    // Si le nom du vin ET son descriptif ne contiennent PAS le texte recherché...
                    if (!vin.NomVin.ToLower().Contains(searchText) && !vin.Descriptif.ToLower().Contains(searchText))
                    {
                        keepThisVin = false; // ...alors on ne garde pas ce vin.
                    }
                }

                // --- TEST 2 : Le type de vin ---
                if (cmbTypeVin.SelectedItem is string selectedType && selectedType != "Tous les types")
                {
                    // Si le type du vin est différent de celui sélectionné...
                    if (vin.UnType.NomType != selectedType)
                    {
                        keepThisVin = false; // ...on ne le garde pas.
                    }
                }

                // --- TEST 3 : L'appellation ---
                if (cmbAppellation.SelectedItem is string selectedAppellation && selectedAppellation != "Toutes appellations")
                {
                    if (vin.UneAppelation.Nomappelation != selectedAppellation)
                    {
                        keepThisVin = false;
                    }
                }

                // --- TEST 4 : L'année (Millésime) ---
                if (int.TryParse(txtAnnee.Text, out int annee) && annee > 0)
                {
                    if (vin.Millesime != annee)
                    {
                        keepThisVin = false;
                    }
                }

                // --- TEST 5 : Le prix maximum ---
                if (decimal.TryParse(txtPrixMax.Text, out decimal prixMax) && prixMax > 0)
                {
                    if (vin.PrixVin > prixMax)
                    {
                        keepThisVin = false;
                    }
                }

                // Si, après tous les tests, la variable keepThisVin est toujours à 'true'...
                if (keepThisVin)
                {
                    // ...cela signifie que le vin a passé tous les filtres, donc on l'ajoute à notre liste de résultats.
                    updatedList.Add(vin);
                }
            }

            // Pour finir, on met à jour l'interface graphique.
            FilteredVins.Clear(); // On vide la liste affichée
            foreach (var vin in updatedList) // Et on la remplit avec nos résultats filtrés
            {
                FilteredVins.Add(vin);
            }
        }

        private void Filters_Changed(object sender, RoutedEventArgs e)
        {
            if (_isResetting)
            {
                return;
            }
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
        // FIN : AJOUT DE LA LOGIQUE POUR LE PANNEAU DE DÉTAILS            
        // ===============================================================
    }

}
