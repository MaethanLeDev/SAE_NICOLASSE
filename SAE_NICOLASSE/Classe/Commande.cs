using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TD3_BindingBDPension.Model;

namespace SAE_NICOLASSE.Classe
{
    public class Commande : ICrud<Commande>, INotifyPropertyChanged
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
        public Commande() { }

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

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            throw new NotImplementedException();
        }

        public int Delete()
        {
            throw new NotImplementedException();
        }

        public List<Commande> FindAll()
        {
            List<Commande> lesCommandes = new List<Commande>();
            try
            {
                // Requête qui joint COMMANDE, EMPLOYE et ROLE pour récupérer toutes les informations nécessaires.
                string sql = @"
                SELECT 
                    c.numcommande,
                    c.datecommande,
                    c.valider,
                    c.prixtotal,
                    e.numemploye,
                    e.nom,
                    e.prenom,
                    e.login,
                    e.mdp,
                    r.numrole,
                    r.nomrole
                FROM 
                    COMMANDE c
                JOIN 
                    EMPLOYE e ON c.numemploye = e.numemploye
                JOIN 
                    ROLE r ON e.numrole = r.numrole
                ORDER BY 
                    c.datecommande DESC;";

                using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
                {
                    // Exécution de la requête via votre couche d'accès
                    DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);

                    // Itération sur chaque ligne du résultat
                    foreach (DataRow dr in dt.Rows)
                    {
                        // 1. Construire l'objet Role
                        Role leRole = new Role(
                            Convert.ToInt32(dr["numrole"]),
                            dr["nomrole"].ToString()
                        );

                        // 2. Construire l'objet Employe en utilisant le Role créé juste avant
                        Employe lEmploye = new Employe(
                            Convert.ToInt32(dr["numemploye"]),
                            leRole,
                            dr["nom"].ToString(),
                            dr["prenom"].ToString(),
                            dr["login"].ToString(),
                            dr["mdp"].ToString()
                        );

                        // 3. Finalement, construire l'objet Commande
                        Commande laCommande = new Commande(
                            Convert.ToInt32(dr["numcommande"]),
                            lEmploye, // On passe l'objet Employe complet
                            Convert.ToDateTime(dr["datecommande"]),
                            Convert.ToBoolean(dr["valider"]),
                            Convert.ToDecimal(dr["prixtotal"])
                        );

                        lesCommandes.Add(laCommande);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des commandes : " + ex.Message);
            }

            return lesCommandes;
        }

        public List<Commande> FindBySelection(string criteres)
        {
            throw new NotImplementedException();
        }

        public void Read()
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
