using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace SAE_NICOLASSE.Classe
{
    public class Demande
    {

        private int numDemande;
        private Vin unVin;
        private Employe unEmploye;
        private Commande numCommande;
        private Client numClient;
        private DateTime dateDemande;
        private int quantiteDemande;
        private string accepter;

        public Demande(int numDemande, Vin unVin, Employe unEmploye, Commande numCommande, Client numClient, DateTime dateDemande, int quantiteDemande, string accepter)
        {
            this.NumDemande = numDemande;
            this.UnVin = unVin;
            this.UnEmploye = unEmploye;
            this.NumCommande = numCommande;
            this.NumClient = numClient;
            this.DateDemande = dateDemande;
            this.QuantiteDemande = quantiteDemande;
            this.Accepter = accepter;
        }

        public int NumDemande
        {
            get
            {
                return this.numDemande;
            }

            set
            {
                this.numDemande = value;
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

        public Commande NumCommande
        {
            get
            {
                return this.numCommande;
            }

            set
            {
                this.numCommande = value;
            }
        }

        public Client NumClient
        {
            get
            {
                return this.numClient;
            }

            set
            {
                this.numClient = value;
            }
        }

        public DateTime DateDemande
        {
            get
            {
                return this.dateDemande;
            }

            set
            {
                this.dateDemande = value;
            }
        }

        public int QuantiteDemande
        {
            get
            {
                return this.quantiteDemande;
            }

            set
            {
                this.quantiteDemande = value;
            }
        }

        public string Accepter
        {
            get
            {
                return this.accepter;
            }

            set
            {
                this.accepter = value;
            }
        }
    }
}
