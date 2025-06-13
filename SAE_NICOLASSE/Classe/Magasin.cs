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
        public ObservableCollection<Demande> LesDemandes { get; set; }
        public ObservableCollection<Vin> LesVins { get; set; }
        public ObservableCollection<Client> LesClients { get; set; }
        public ObservableCollection<Commande> LesCommandes { get; set; }

        /// <summary>
        /// Le constructeur prend maintenant l'objet DataAccess en paramètre
        /// pour charger les données avec les bons droits.
        /// </summary>
        /// <param name="dao">L'instance de DataAccess avec la connexion du bon rôle.</param>
        public Magasin(DataAccess dao)
        {
            // On passe le 'dao' à chaque méthode FindAll
            this.LesDemandes = new ObservableCollection<Demande>(new Demande().FindAll(dao));
            this.LesVins = new ObservableCollection<Vin>(new Vin().FindAll(dao));
            this.LesClients = new ObservableCollection<Client>(new Client().FindAll(dao));
            this.LesCommandes = new ObservableCollection<Commande>(new Commande().FindAll(dao));
        }
    }
}
