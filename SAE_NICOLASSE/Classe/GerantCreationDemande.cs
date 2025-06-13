// ========================================================================
// FICHIER : Classe/GerantCreationDemande.cs
// DÉCISION : Ce fichier vient entièrement du projet de ton collègue.
//            Il est indispensable pour la logique de création de commandes
//            et est donc ajouté au projet fusionné.
// ========================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SAE_NICOLASSE.Classe
{
    /// <summary>
    /// Cette classe sert de "cerveau" pour l'écran de création de commande.
    /// Elle gère les listes de demandes disponibles et les lignes de la commande en cours.
    /// </summary>
    public class GerantCreationDemande
    {
        public ObservableCollection<Demande> DemandesDisponibles { get; set; }
        public ObservableCollection<LigneCommande> VinsDansLaCommande { get; set; }

        public GerantCreationDemande(Magasin leMagasin)
        {
            this.VinsDansLaCommande = new ObservableCollection<LigneCommande>();

            List<Demande> demandesFiltrees = new List<Demande>();
            foreach (Demande demande in leMagasin.LesDemandes)
            {
                // On ne garde que les demandes acceptées qui ne sont pas encore dans une commande.
                if (demande.Accepter == "Accepté" && demande.NumCommande == null)
                {
                    demandesFiltrees.Add(demande);
                }
            }
            this.DemandesDisponibles = new ObservableCollection<Demande>(demandesFiltrees);
        }
    }

    /// <summary>
    /// Représente une ligne dans le panier de commande, regroupant toutes
    /// les demandes pour un même vin.
    /// </summary>
    public class LigneCommande
    {
        public Vin UnVin { get; set; }
        public int QuantiteTotale { get; set; }

        // Garde en mémoire la liste des demandes originales qui composent cette ligne.
        public List<Demande> DemandesComposees { get; set; }

        public LigneCommande(Demande premiereDemande)
        {
            this.UnVin = premiereDemande.UnVin;
            this.QuantiteTotale = premiereDemande.QuantiteDemande;
            this.DemandesComposees = new List<Demande> { premiereDemande };
        }

        // Ajoute une nouvelle demande à une ligne existante.
        public void FusionnerDemande(Demande demandeAFusionner)
        {
            this.QuantiteTotale += demandeAFusionner.QuantiteDemande;
            this.DemandesComposees.Add(demandeAFusionner);
        }
    }
}
