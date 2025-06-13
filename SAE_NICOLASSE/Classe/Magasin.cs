

using System.Collections.ObjectModel;

namespace SAE_NICOLASSE.Classe
{
    public class Magasin
    {
        public ObservableCollection<Demande> LesDemandes { get; set; }
        public ObservableCollection<Vin> LesVins { get; set; }
        public ObservableCollection<Client> LesClients { get; set; }
        public ObservableCollection<Commande> LesCommandes { get; set; }
        public ObservableCollection<Employe> LesEmployes { get; set; }

        public Magasin()
        {
            this.LesDemandes = new ObservableCollection<Demande>(new Demande().FindAll());
            this.LesVins = new ObservableCollection<Vin>(new Vin().FindAll());
            this.LesClients = new ObservableCollection<Client>(new Client().FindAll());
            this.LesCommandes = new ObservableCollection<Commande>(new Commande().FindAll());
            this.LesEmployes = new ObservableCollection<Employe>(new Employe().FindAll());
        }
    }
}
