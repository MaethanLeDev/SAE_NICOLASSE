// ========================================================================
// FICHIER : Classe/Commande.cs
// DÉCISION : Je conserve la version de ton collègue ("lui").
//            Elle est fonctionnellement complète, notamment avec la
//            méthode Delete() qui utilise une transaction pour supprimer
//            proprement une commande et ses dépendances.
// ========================================================================

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

        public int Delete()
        {
            // Utilisation d'une transaction pour garantir l'intégrité des données.
            // Soit tout est supprimé, soit rien ne l'est.
            using (var connection = DataAccess.Instance.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // 1. Délier les demandes associées
                    string sqlUnlinkDemandes = "UPDATE DEMANDE SET numcommande = NULL WHERE numcommande = @id";
                    using (var cmdUnlink = new NpgsqlCommand(sqlUnlinkDemandes, connection, transaction))
                    {
                        cmdUnlink.Parameters.AddWithValue("@id", this.Numcommande);
                        cmdUnlink.ExecuteNonQuery();
                    }

                    // 2. Supprimer les détails de la commande
                    string sqlDeleteDetails = "DELETE FROM DETAILCOMMANDE WHERE numcommande = @id";
                    using (var cmdDetails = new NpgsqlCommand(sqlDeleteDetails, connection, transaction))
                    {
                        cmdDetails.Parameters.AddWithValue("@id", this.Numcommande);
                        cmdDetails.ExecuteNonQuery();
                    }

                    // 3. Supprimer la commande principale
                    string sqlDeleteCommande = "DELETE FROM COMMANDE WHERE numcommande = @id";
                    using (var cmdCommande = new NpgsqlCommand(sqlDeleteCommande, connection, transaction))
                    {
                        cmdCommande.Parameters.AddWithValue("@id", this.Numcommande);
                        int rowsAffected = cmdCommande.ExecuteNonQuery();

                        // Si tout s'est bien passé, on valide la transaction
                        transaction.Commit();
                        return rowsAffected;
                    }
                }
                catch (Exception ex)
                {
                    // En cas d'erreur, on annule toutes les opérations
                    transaction.Rollback();
                    MessageBox.Show("Erreur lors de la suppression de la commande : " + ex.Message);
                    return 0;
                }
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

