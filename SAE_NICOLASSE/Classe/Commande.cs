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
        public int Numcommande { get; set; }
        public Employe UnEmploye { get; set; }
        public DateTime DateCommande { get; set; }
        public bool Valider { get; set; }
        public decimal Prixtotal { get; set; }

        public Commande(int numcommande, Employe unEmploye, DateTime dateCommande, bool valider, decimal prixtotal)
        {
            this.Numcommande = numcommande; this.UnEmploye = unEmploye; this.DateCommande = dateCommande; this.Valider = valider; this.Prixtotal = prixtotal;
        }
        public Commande() { }

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<Commande> FindAll(DataAccess dao)
        {
            List<Commande> lesCommandes = new List<Commande>();
            string sql = @"
                SELECT 
                    c.numcommande, c.datecommande, c.valider, c.prixtotal,
                    e.numemploye, e.nom, e.prenom, e.login, e.mdp,
                    r.numrole, r.nomrole
                FROM COMMANDE c
                JOIN EMPLOYE e ON c.numemploye = e.numemploye
                JOIN ROLE r ON e.numrole = r.numrole
                ORDER BY c.datecommande DESC;";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
            {
                DataTable dt = dao.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    Role leRole = new Role(Convert.ToInt32(dr["numrole"]), dr["nomrole"].ToString());
                    Employe lEmploye = new Employe(Convert.ToInt32(dr["numemploye"]), leRole, dr["nom"].ToString(), dr["prenom"].ToString(), dr["login"].ToString(), dr["mdp"].ToString());

                    Commande laCommande = new Commande(
                        Convert.ToInt32(dr["numcommande"]),
                        lEmploye,
                        Convert.ToDateTime(dr["datecommande"]),
                        Convert.ToBoolean(dr["valider"]),
                        Convert.ToDecimal(dr["prixtotal"])
                    );
                    lesCommandes.Add(laCommande);
                }
            }
            return lesCommandes;
        }

        public int Create(DataAccess dao) { throw new NotImplementedException(); }
        public int Delete(DataAccess dao) { throw new NotImplementedException(); }
        public List<Commande> FindBySelection(string criteres, DataAccess dao) { throw new NotImplementedException(); }
        public void Read(DataAccess dao) { throw new NotImplementedException(); }
        public int Update(DataAccess dao) { throw new NotImplementedException(); }
    }
}
