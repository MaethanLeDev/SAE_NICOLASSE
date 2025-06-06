using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE_NICOLASSE.Classe
{
    public class DetailCommande
    {
        private Commande uneCommande;
        private Vin unVin;
        private int quantite;
        private decimal prix;

        public DetailCommande(Commande uneCommande, Vin unVin, int quantite, decimal prix)
        {
            this.UneCommande = uneCommande;
            this.UnVin = unVin;
            this.Quantite = quantite;
            this.Prix = prix;
        }

        public Commande UneCommande
        {
            get
            {
                return this.uneCommande;
            }

            set
            {
                this.uneCommande = value;
            }
        }

        public Vin UnVin
        {
            get
            {
                return this.unVin;
            }

            set
            {
                this.unVin = value;
            }
        }

        public int Quantite
        {
            get
            {
                return this.quantite;
            }

            set
            {
                this.quantite = value;
            }
        }

        public decimal Prix
        {
            get
            {
                return this.prix;
            }

            set
            {
                this.prix = value;
            }
        }
    }
}
