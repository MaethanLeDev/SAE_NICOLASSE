using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SAE_NICOLASSE.Classe
{
    public class Magasin
    {
        private ObservableCollection<Demande> lesDemandes;
        private ObservableCollection<Vin> lesVins;
        private ObservableCollection<Client> lesClients;
        private ObservableCollection<Commande> lesCommandes;

        public Magasin()
        {
            this.LesDemandes = new ObservableCollection<Demande>(new Demande().FindAll());
            this.LesVins = new ObservableCollection<Vin>(new Vin().FindAll()); ;
            this.LesClients = new ObservableCollection<Client>(new Client().FindAll()); ;
            this.LesCommandes = new ObservableCollection<Commande>(new Commande().FindAll()); ; 
        }

        public ObservableCollection<Demande> LesDemandes
        {
            get
            {
                return this.lesDemandes;
            }

            set
            {
                this.lesDemandes = value;
            }
        }

        public ObservableCollection<Vin> LesVins
        {
            get
            {
                return this.lesVins;
            }

            set
            {
                this.lesVins = value;
            }
        }

        public ObservableCollection<Client> LesClients
        {
            get
            {
                return this.lesClients;
            }

            set
            {
                this.lesClients = value;
            }
        }

        public ObservableCollection<Commande> LesCommandes
        {
            get
            {
                return this.lesCommandes;
            }

            set
            {
                this.lesCommandes = value;
            }
        }
    }
}
