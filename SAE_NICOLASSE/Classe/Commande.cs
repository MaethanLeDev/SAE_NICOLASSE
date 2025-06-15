using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            this.Numcommande = numcommande;
            this.UnEmploye = unEmploye;
            this.DateCommande = dateCommande;
            this.Valider = valider;
            this.Prixtotal = prixtotal;
        }
        public Commande() { }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            string sql = @"
            INSERT INTO COMMANDE (numemploye, datecommande, valider, prixtotal) 
            VALUES (@numemploye, @datecommande, @valider, @prixtotal)
            RETURNING numcommande;";

            using (var cmd = new NpgsqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@numemploye", this.UnEmploye.NumEmploye);
                cmd.Parameters.AddWithValue("@datecommande", this.DateCommande);
                cmd.Parameters.AddWithValue("@valider", this.Valider);
                cmd.Parameters.AddWithValue("@prixtotal", this.Prixtotal);

                return DataAccess.Instance.ExecuteInsert(cmd);
            }
        }

        // Dans la classe Commande.cs

        public int Delete()
        {
            try
            {
                // Tâche 1: On délie les demandes associées.
                // On prépare la commande SQL et on l'envoie avec ExecuteSet.
                string sqlUnlinkDemandes = "UPDATE DEMANDE SET numcommande = NULL WHERE numcommande = @id";
                using (var cmdUnlink = new NpgsqlCommand(sqlUnlinkDemandes))
                {
                    cmdUnlink.Parameters.AddWithValue("@id", this.Numcommande);
                    DataAccess.Instance.ExecuteSet(cmdUnlink);
                }

                // Tâche 2: On supprime les détails de la commande.
                string sqlDeleteDetails = "DELETE FROM DETAILCOMMANDE WHERE numcommande = @id";
                using (var cmdDetails = new NpgsqlCommand(sqlDeleteDetails))
                {
                    cmdDetails.Parameters.AddWithValue("@id", this.Numcommande);
                    DataAccess.Instance.ExecuteSet(cmdDetails);
                }

                // Tâche 3: On supprime la commande principale.
                string sqlDeleteCommande = "DELETE FROM COMMANDE WHERE numcommande = @id";
                using (var cmdCommande = new NpgsqlCommand(sqlDeleteCommande))
                {
                    cmdCommande.Parameters.AddWithValue("@id", this.Numcommande);

                    // On exécute la dernière commande et on retourne le résultat.
                    return DataAccess.Instance.ExecuteSet(cmdCommande);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression de la commande : " + ex.Message);
                return 0; // On retourne 0 pour signifier qu'il y a eu un échec.
            }
        }


        public List<Commande> FindAll()
        {
            List<Commande> lesCommandes = new List<Commande>();
            try
            {
                string sql = @"
                SELECT 
                    c.numcommande, c.datecommande, c.valider, c.prixtotal,
                    e.numemploye, e.nom, e.prenom, e.login, e.mdp,
                    r.numrole, r.nomrole
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
                    DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des commandes : " + ex.Message);
            }
            return lesCommandes;
        }

        public List<Commande> FindBySelection(string criteres) { throw new NotImplementedException(); }
        public void Read() { throw new NotImplementedException(); }
        public int Update()
        {
            // Requête SQL pour mettre à jour le statut "valider" d'une commande spécifique.
            string sql = @"UPDATE COMMANDE SET valider = @valider WHERE numcommande = @numcommande";
            using (var cmd = new NpgsqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@valider", this.Valider);
                cmd.Parameters.AddWithValue("@numcommande", this.Numcommande);

                // Assurez-vous d'utiliser la bonne méthode de votre DataAccess
                // pour les UPDATE (celle qui utilise ExecuteNonQuery).
                return DataAccess.Instance.ExecuteSet(cmd);
            }
        }
        }
    }

