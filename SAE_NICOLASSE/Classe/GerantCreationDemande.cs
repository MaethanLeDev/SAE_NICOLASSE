using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SAE_NICOLASSE.Classe
{
    
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
               
                if (demande.Accepter == "Accepté" && demande.NumCommande == null)
                {
                    demandesFiltrees.Add(demande);
                }
            }
            this.DemandesDisponibles = new ObservableCollection<Demande>(demandesFiltrees);
        }
    }

    
    public class LigneCommande
    {
        public Vin UnVin { get; set; }
        public int QuantiteTotale { get; set; }

        
        public List<Demande> DemandesComposees { get; set; }

        public LigneCommande(Demande premiereDemande)
        {
            this.UnVin = premiereDemande.UnVin;
            this.QuantiteTotale = premiereDemande.QuantiteDemande;
            this.DemandesComposees = new List<Demande> { premiereDemande };
        }

       
        public void FusionnerDemande(Demande demandeAFusionner)
        {
            this.QuantiteTotale += demandeAFusionner.QuantiteDemande;
            this.DemandesComposees.Add(demandeAFusionner);
        }
    }
}
