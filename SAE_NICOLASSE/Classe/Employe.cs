using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using TD3_BindingBDPension.Model;

namespace SAE_NICOLASSE.Classe
{
    public class Employe : ICrud<Employe>, INotifyPropertyChanged
    {
        public int NumEmploye { get; set; }
        public Role UnRole { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Login { get; set; }
        public string Mdp { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Employe() { }

        public Employe(int numEmploye, Role unRole, string nom, string prenom, string login, string mdp)
        {
            this.NumEmploye = numEmploye;
            this.UnRole = unRole;
            this.Nom = nom;
            this.Prenom = prenom;
            this.Login = login;
            this.Mdp = mdp;
        }

        public Employe FindByLogin(string login)
        {
            string sql = "SELECT * FROM employe e JOIN role r ON e.numrole = r.numrole WHERE e.login = @login";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@login", login);

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    Role leRole = new Role(Convert.ToInt32(dr["numrole"]), dr["nomrole"].ToString());
                    return new Employe(
                        Convert.ToInt32(dr["numemploye"]),
                        leRole,
                        dr["nom"].ToString(),
                        dr["prenom"].ToString(),
                        dr["login"].ToString(),
                        dr["mdp"].ToString()
                    );
                }
            }
            return null;
        }

        public List<Employe> FindAll()
        {
            List<Employe> lesEmployes = new List<Employe>();
            string sql = @"
                SELECT 
                    e.numemploye, e.nom, e.prenom, e.login, e.mdp,
                    r.numrole, r.nomrole
                FROM 
                    EMPLOYE e
                JOIN 
                    ROLE r ON e.numrole = r.numrole
                ORDER BY 
                    e.nom, e.prenom;";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);

                foreach (DataRow dr in dt.Rows)
                {
                    Role leRole = new Role(
                        Convert.ToInt32(dr["numrole"]),
                        dr["nomrole"].ToString()
                    );

                    Employe lEmploye = new Employe(
                        Convert.ToInt32(dr["numemploye"]),
                        leRole,
                        dr["nom"].ToString(),
                        dr["prenom"].ToString(),
                        dr["login"].ToString(),
                        dr["mdp"].ToString()
                    );
                    lesEmployes.Add(lEmploye);
                }
            }
            return lesEmployes;
        }

        public int Create() { throw new NotImplementedException(); }
        public int Delete() { throw new NotImplementedException(); }
        public List<Employe> FindBySelection(string criteres) { throw new NotImplementedException(); }
        public void Read() { throw new NotImplementedException(); }
        public int Update() { throw new NotImplementedException(); }
    }
}
