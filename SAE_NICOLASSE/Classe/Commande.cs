using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SAE_NICOLASSE.Classe
{
    public class Commande
    {
        private int numcommande;
        private Employe unEmploye;
        private DateTime dateCommande;
        private bool valider;
        private decimal prixtotal;

        public Commande(int numcommande, Employe unEmploye, DateTime dateCommande, bool valider, decimal prixtotal)
        {
            this.Numcommande = numcommande;
            this.UnEmploye = unEmploye;
            this.DateCommande = dateCommande;
            this.Valider = valider;
            this.Prixtotal = prixtotal;
        }

        public int Numcommande
        {
            get
            {
                return this.numcommande;
            }

            set
            {
                this.numcommande = value;
            }
        }

        public Employe UnEmploye
        {
            get
            {
                return this.unEmploye;
            }

            set
            {
                this.unEmploye = value;
            }
        }

        public DateTime DateCommande
        {
            get
            {
                return this.dateCommande;
            }

            set
            {
                this.dateCommande = value;
            }
        }

        public bool Valider
        {
            get
            {
                return this.valider;
            }

            set
            {
                this.valider = value;
            }
        }

        public decimal Prixtotal
        {
            get
            {
                return this.prixtotal;
            }

            set
            {
                this.prixtotal = value;
            }
        }
    }
}
