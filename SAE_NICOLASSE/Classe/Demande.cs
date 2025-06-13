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
using System.Windows.Markup;
using TD3_BindingBDPension.Model;

namespace SAE_NICOLASSE.Classe
{
    public class Demande : ICrud<Demande>, INotifyPropertyChanged
    {
        public int NumDemande { get; set; }
        public Vin UnVin { get; set; }
        public Employe UnEmploye { get; set; }
        public Commande NumCommande { get; set; }
        public Client NumClient { get; set; }
        public DateTime DateDemande { get; set; }
        public int QuantiteDemande { get; set; }
        public string Accepter { get; set; }

        public Demande(int numDemande, Vin unVin, Employe unEmploye, Commande numCommande, Client numClient, DateTime dateDemande, int quantiteDemande, string accepter)
        {
            this.NumDemande = numDemande; this.UnVin = unVin; this.UnEmploye = unEmploye; this.NumCommande = numCommande; this.NumClient = numClient; this.DateDemande = dateDemande; this.QuantiteDemande = quantiteDemande; this.Accepter = accepter;
        }
        public Demande() { }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Create(DataAccess dao)
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

                return dao.ExecuteInsert(cmdInsert);
            }
        }

        public List<Demande> FindAll(DataAccess dao)
        {
            List<Demande> lesDemandes = new List<Demande>();
            string sql = @"
                SELECT
                    d.numdemande, d.datedemande, d.quantitedemande, d.accepter,
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
                    a.nomappelation,
                    c.numcommande, c.datecommande, c.valider, c.prixtotal,
                    e_cde.numemploye    AS cde_employe_num,
                    e_cde.nom           AS cde_employe_nom,
                    e_cde.prenom        AS cde_employe_prenom,
                    e_cde.login         AS cde_employe_login,
                    e_cde.mdp           AS cde_employe_mdp,
                    r_cde.numrole       AS cde_role_num,
                    r_cde.nomrole       AS cde_role_nom
                FROM
                    DEMANDE d
                    JOIN CLIENT cl ON d.numclient = cl.numclient
                    JOIN VIN v ON d.numvin = v.numvin
                    JOIN EMPLOYE e_dem ON d.numemploye = e_dem.numemploye
                    JOIN FOURNISSEUR f ON v.numfournisseur = f.numfournisseur
                    JOIN TYPEVIN tv ON v.numtype = tv.numtype
                    JOIN APPELATION a ON v.numtype2 = a.numtype
                    JOIN ROLE r_dem ON e_dem.numrole = r_dem.numrole
                    LEFT JOIN COMMANDE c ON d.numcommande = c.numcommande
                    LEFT JOIN EMPLOYE e_cde ON c.numemploye = e_cde.numemploye
                    LEFT JOIN ROLE r_cde ON e_cde.numrole = r_cde.numrole
                ORDER BY d.datedemande DESC;";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
            {
                DataTable dt = dao.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    Client leClient = new Client(Convert.ToInt32(dr["numclient"]), dr["nomclient"].ToString(), dr["prenomclient"].ToString(), dr["mailclient"].ToString());
                    Role roleDemandeur = new Role(Convert.ToInt32(dr["demandeur_role_num"]), dr["demandeur_role_nom"].ToString());
                    Employe employeDemandeur = new Employe(Convert.ToInt32(dr["demandeur_num"]), roleDemandeur, dr["demandeur_nom"].ToString(), dr["demandeur_prenom"].ToString(), dr["demandeur_login"].ToString(), dr["demandeur_mdp"].ToString());
                    Fournisseur leFournisseur = new Fournisseur(Convert.ToInt32(dr["numfournisseur"]), dr["nomfournisseur"].ToString());
                    TypeVin leTypeVin = new TypeVin(Convert.ToInt32(dr["numtype"]), dr["nomtype"].ToString());
                    Appelation lAppelation = new Appelation(Convert.ToInt32(dr["appelation_num"]), dr["nomappelation"].ToString());
                    Vin leVin = new Vin(Convert.ToInt32(dr["numvin"]), leFournisseur, leTypeVin, lAppelation, dr["nomvin"].ToString(), Convert.ToDecimal(dr["prixvin"]), dr["descriptif"].ToString(), Convert.ToInt32(dr["millesime"]));
                    Commande laCommande = null;
                    if (dr["numcommande"] != DBNull.Value)
                    {
                        Role roleCommande = new Role(Convert.ToInt32(dr["cde_role_num"]), dr["cde_role_nom"].ToString());
                        Employe employeCommande = new Employe(Convert.ToInt32(dr["cde_employe_num"]), roleCommande, dr["cde_employe_nom"].ToString(), dr["cde_employe_prenom"].ToString(), dr["cde_employe_login"].ToString(), dr["cde_employe_mdp"].ToString());
                        laCommande = new Commande(Convert.ToInt32(dr["numcommande"]), employeCommande, Convert.ToDateTime(dr["datecommande"]), Convert.ToBoolean(dr["valider"]), Convert.ToDecimal(dr["prixtotal"]));
                    }
                    Demande laDemande = new Demande(Convert.ToInt32(dr["numdemande"]), leVin, employeDemandeur, laCommande, leClient, Convert.ToDateTime(dr["datedemande"]), Convert.ToInt32(dr["quantitedemande"]), dr["accepter"].ToString());
                    lesDemandes.Add(laDemande);
                }
            }
            return lesDemandes;
        }

        public int Update(DataAccess dao)
        {
            string sql = @"UPDATE DEMANDE 
                           SET ACCEPTER = @accepter 
                           WHERE NUMDEMANDE = @numdemande";

            using (var cmdUpdate = new NpgsqlCommand(sql))
            {
                cmdUpdate.Parameters.AddWithValue("@accepter", this.Accepter);
                cmdUpdate.Parameters.AddWithValue("@numdemande", this.NumDemande);
                return dao.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete(DataAccess dao) { throw new NotImplementedException(); }
        public List<Demande> FindBySelection(string criteres, DataAccess dao) { throw new NotImplementedException(); }
        public void Read(DataAccess dao) { throw new NotImplementedException(); }
    }
}