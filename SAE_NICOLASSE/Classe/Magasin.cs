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

        public Magasin(ObservableCollection<Demande> lesDemandes, ObservableCollection<Vin> lesVins, ObservableCollection<Client> lesClients, ObservableCollection<Commande> lesCommandes)
        {
            this.LesDemandes = lesDemandes;
            this.LesVins = lesVins;
            this.LesClients = lesClients;
            this.LesCommandes = lesCommandes;
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
