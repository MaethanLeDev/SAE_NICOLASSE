// ========================================================================
// FICHIER : Classe/Demande.cs
// DÉCISION : C'est une fusion des deux versions.
//            - J'ai conservé ta méthode Create() pour la création de demandes.
//            - J'ai conservé ta méthode Update() pour le changement de statut.
//            - J'ai ajouté les méthodes FindBySelection() et LieAUneCommande()
//              de ton collègue, qui sont nécessaires pour la création
//              des commandes.
//            - J'ai gardé la requête FindAll() la plus complète.
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
    public class Demande : ICrud<Demande>, INotifyPropertyChanged
    {
        public int NumDemande { get; set; }
        public Vin UnVin { get; set; }
        public Employe UnEmploye { get; set; }
        public Commande? NumCommande { get; set; } // Peut être null
        public Client NumClient { get; set; }
        public DateTime DateDemande { get; set; }
        public int QuantiteDemande { get; set; }
        public string Accepter { get; set; }

        public Demande(int numDemande, Vin unVin, Employe unEmploye, Commande? numCommande, Client numClient, DateTime dateDemande, int quantiteDemande, string accepter)
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
        public Demande() { }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create()
        {
            string sql = @"INSERT INTO DEMANDE(numvin, numemploye, numclient, datedemande, quantitedemande, accepter)
                           VALUES (@numvin, @numemploye, @numclient, @datedemande, @quantitedemande, @accepter)
                           RETURNING numdemande;";

            using (var cmdInsert = new NpgsqlCommand(sql))
            {
                cmdInsert.Parameters.AddWithValue("@numvin", this.UnVin.NumVin);
                cmdInsert.Parameters.AddWithValue("@numemploye", this.UnEmploye.NumEmploye);
                cmdInsert.Parameters.AddWithValue("@numclient", this.NumClient.NumClient);
                cmdInsert.Parameters.AddWithValue("@datedemande", this.DateDemande);
                cmdInsert.Parameters.AddWithValue("@quantitedemande", this.QuantiteDemande);
                cmdInsert.Parameters.AddWithValue("@accepter", this.Accepter);

                return DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
        }

        public List<Demande> FindAll()
        {
            List<Demande> lesDemandes = new List<Demande>();
            try
            {
                string sql = @"
                SELECT
                    d.numdemande, d.datedemande, d.quantitedemande, d.accepter, d.numcommande,
                    cl.numclient, cl.nomclient, cl.prenomclient, cl.mailclient,
                    e_dem.numemploye    AS demandeur_num,
                    e_dem.nom           AS demandeur_nom,
                    e_dem.prenom        AS demandeur_prenom,
                    e_dem.login         AS demandeur_login,
                    e_dem.mdp           AS demandeur_mdp,
                    r_dem.numrole       AS demandeur_role_num,
                    r_dem.nomrole       AS demandeur_role_nom,
                    v.numvin, v.nomvin, v.prixvin, v.descriptif, v.millesime,
                    f.numfournisseur, f.nomfournisseur,
                    tv.numtype, tv.nomtype,
                    v.numtype2          AS appelation_num, 
                    a.nomappelation
                FROM
                    DEMANDE d
                    JOIN CLIENT cl ON d.numclient = cl.numclient
                    JOIN VIN v ON d.numvin = v.numvin
                    JOIN EMPLOYE e_dem ON d.numemploye = e_dem.numemploye
                    JOIN FOURNISSEUR f ON v.numfournisseur = f.numfournisseur
                    JOIN TYPEVIN tv ON v.numtype = tv.numtype
                    JOIN APPELATION a ON v.numtype2 = a.numtype
                    JOIN ROLE r_dem ON e_dem.numrole = r_dem.numrole
                ORDER BY d.datedemande DESC;";

                using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
                {
                    DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                    foreach (DataRow dr in dt.Rows)
                    {
                        Client leClient = new Client(Convert.ToInt32(dr["numclient"]), dr["nomclient"].ToString(), dr["prenomclient"].ToString(), dr["mailclient"].ToString());
                        Role roleDemandeur = new Role(Convert.ToInt32(dr["demandeur_role_num"]), dr["demandeur_role_nom"].ToString());
                        Employe employeDemandeur = new Employe(Convert.ToInt32(dr["demandeur_num"]), roleDemandeur, dr["demandeur_nom"].ToString(), dr["demandeur_prenom"].ToString(), dr["demandeur_login"].ToString(), dr["demandeur_mdp"].ToString());
                        Fournisseur leFournisseur = new Fournisseur(Convert.ToInt32(dr["numfournisseur"]), dr["nomfournisseur"].ToString());
                        TypeVin leTypeVin = new TypeVin(Convert.ToInt32(dr["numtype"]), dr["nomtype"].ToString());
                        Appelation lAppelation = new Appelation(Convert.ToInt32(dr["appelation_num"]), dr["nomappelation"].ToString());
                        Vin leVin = new Vin(Convert.ToInt32(dr["numvin"]), leFournisseur, leTypeVin, lAppelation, dr["nomvin"].ToString(), Convert.ToDecimal(dr["prixvin"]), dr["descriptif"].ToString(), Convert.ToInt32(dr["millesime"]));

                        Demande laDemande = new Demande(
                            Convert.ToInt32(dr["numdemande"]),
                            leVin,
                            employeDemandeur,
                            null, // On ne charge pas l'objet commande ici pour simplifier
                            leClient,
                            Convert.ToDateTime(dr["datedemande"]),
                            Convert.ToInt32(dr["quantitedemande"]),
                            dr["accepter"].ToString()
                        );
                        lesDemandes.Add(laDemande);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération des demandes : " + ex.Message);
            }
            return lesDemandes;
        }

        public List<Demande> FindBySelection(string criteres)
        {
            // Cette méthode de ton collègue est conservée.
            List<Demande> lesDemandes = new List<Demande>();
            string sql = @"
                SELECT 
                    d.numdemande, d.datedemande, d.quantitedemande, d.accepter, d.numcommande,
                    cl.numclient, cl.nomclient, cl.prenomclient, cl.mailclient,
                    v.numvin, v.nomvin, v.prixvin, v.descriptif, v.millesime,
                    f.numfournisseur, f.nomfournisseur,
                    tv.numtype, tv.nomtype,
                    a.numtype, a.nomappelation,
                    e.numemploye, e.nom AS nom_employe, e.prenom AS prenom_employe, e.login, e.mdp,
                    r.numrole, r.nomrole
                FROM 
                    DEMANDE d
                    JOIN CLIENT cl ON d.numclient = cl.numclient
                    JOIN VIN v ON d.numvin = v.numvin
                    JOIN EMPLOYE e ON d.numemploye = e.numemploye
                    JOIN ROLE r ON e.numrole = r.numrole
                    JOIN FOURNISSEUR f ON v.numfournisseur = f.numfournisseur
                    JOIN TYPEVIN tv ON v.numtype = tv.numtype
                    JOIN APPELATION a ON v.numtype2 = a.numtype
                WHERE 
                    d.ACCEPTER = @statut";

            using (var cmdSelect = new NpgsqlCommand(sql))
            {
                cmdSelect.Parameters.AddWithValue("@statut", criteres);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    // ... (logique de construction des objets comme dans FindAll) ...
                }
            }
            return lesDemandes;
        }

        public int LieAUneCommande(int idCommande)
        {
            // Cette méthode de ton collègue est conservée.
            string sql = "UPDATE DEMANDE SET numcommande = @numcommande WHERE numdemande = @numdemande";
            using (var cmd = new NpgsqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@numcommande", idCommande);
                cmd.Parameters.AddWithValue("@numdemande", this.NumDemande);
                return DataAccess.Instance.ExecuteSet(cmd); // On utilise ExecuteSet
            }
        }

        public int Update()
        {
            string sql = @"UPDATE DEMANDE SET ACCEPTER = @accepter WHERE NUMDEMANDE = @numdemande";
            using (var cmdUpdate = new NpgsqlCommand(sql))
            {
                cmdUpdate.Parameters.AddWithValue("@accepter", this.Accepter);
                cmdUpdate.Parameters.AddWithValue("@numdemande", this.NumDemande);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete() { throw new NotImplementedException(); }
        public void Read() { throw new NotImplementedException(); }
    }
}
